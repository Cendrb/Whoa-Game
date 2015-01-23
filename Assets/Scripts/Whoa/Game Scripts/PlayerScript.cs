using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using Aspects.Self.Effects;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    public CollectiblesPrefabs Collectibles { get; set; }
    public AreaEffectsData AreaEffects { get; set; }

    public AudioClip flapSound;
    public AudioClip deathSound;
    public AudioClip crashSound;
    public AudioClip obstaclePassedSound;
    public AudioClip startupSound;
    public AudioClip selfSpellCastedSound;

    public Slider HealthSlider { get; set; }
    public Slider KlidSlider { get; set; }

    public Text HealthText { get; set; }
    public Text KlidText { get; set; }
    public Text ScoreText { get; set; }

    public GameObject ActiveEffectPrefab { get; set; }
    public GameObject ActiveEffectsContainer { get; set; }
    public GameObject CastSelfSpellButtonPrefab { get; set; }
    public Color EnoughKlidToCastColor { get; set; }
    public Color InsufficientKlidToCastColor { get; set; }

    public int additionalAD;

    public GameObject CanvasParent { get; set; }

    public OnPlayerPassedExecutorScript CollectiblesGenerator { get; set; }

    public Transform Follower { get; set; }

    List<Text> spellKlidCostAmountsOnButtons = new List<Text>();

    Dictionary<int, GameObject> selfSpellButtons;

    int obstaclesPassedCount;
    int openAreasPassedCount;
    PlayerDynamicProperties properties;
    bool flapped = false;
    bool selfSpellCasted = false;
    bool bouncedOff = false;
    ParticleSystem[] particles;

    int numberOf100s = 0;

    // Use this for initialization
    private void Start()
    {
        // Generic character initialization
        gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 6;
        rigidbody2D.drag = 1;
        rigidbody2D.fixedAngle = true;
        rigidbody2D.angularDrag = 0.05f;
        ((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Characters/Generic/Farts"))).transform.parent = gameObject.transform;
        gameObject.transform.localScale = new Vector3(0.84f, 0.84f, 0.84f);

        audio.PlayOneShot(startupSound);
        WhoaPlayerProperties.Load();
        WhoaPlayerProperties.LastScore = 0;
        WhoaPlayerProperties.LastMoney = 0;
        properties = new PlayerDynamicProperties(WhoaPlayerProperties.Character);
        properties.HealthChanged += RefreshHealthLabel;
        properties.KlidChanged += RefreshKlidCostLabels;
        properties.KlidChanged += RefreshKlidLabel;
        rigidbody2D.mass = WhoaPlayerProperties.Character.Mass;
        particles = GetComponentsInChildren<ParticleSystem>();

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = WhoaPlayerProperties.Character.Sprite;

        particles[0].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;

        particles[1].emissionRate = (int)WhoaPlayerProperties.Character.Speed;

        foreach (KeyValuePair<int, SelfSpell> selfSpell in WhoaPlayerProperties.Spells.SelfSpells)
            selfSpell.Value.GetKlidCost();

        selfSpellButtons = new Dictionary<int, GameObject>();
        int counter = 130;
        foreach (KeyValuePair<int, int> index in WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds)
        {
            SelfSpell spell = WhoaPlayerProperties.Spells.SelfSpells[index.Value];
            spell.GenerateEffects();
            GameObject button = (GameObject)Instantiate(CastSelfSpellButtonPrefab);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.SetParent(CanvasParent.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector2(120, counter);
            Text abbreviateText = button.transform.FindChild("Abbreviate").gameObject.GetComponentInChildren<Text>();
            abbreviateText.text = spell.Abbreviate;
            Text klidCostText = button.transform.FindChild("KlidCost").gameObject.GetComponentInChildren<Text>();
            klidCostText.text = spell.GetKlidCost().FormatKlid();
            spellKlidCostAmountsOnButtons.Add(klidCostText);
            selfSpellButtons.Add(index.Value, button);
            counter += 160;
        }

        if (WhoaPlayerProperties.Character.KlidEnergy <= 0)
        {
            KlidSlider.gameObject.SetActive(false);
        }
        else
            StartCoroutine(KlidRegeneration());

        HealthSlider.maxValue = WhoaPlayerProperties.Character.Health;
        HealthSlider.minValue = 0;

        KlidSlider.maxValue = WhoaPlayerProperties.Character.KlidEnergy;
        KlidSlider.minValue = 0;

        CollectiblesGenerator.OnCollisionWithPlayer += GenerateCollectibleGeneratorTriggered;
        CollectiblesGenerator.PositionMovementAfterCollision = new Vector2(WhoaPlayerProperties.Settings.MinimalCollectiblesDistance, 0);

        RefreshHealthLabel();
        RefreshKlidLabel();
        RefreshKlidCostLabels();
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x > WhoaPlayerProperties.LastScore)
        {
            WhoaPlayerProperties.LastScore = (int)transform.position.x;
            ScoreText.text = WhoaPlayerProperties.LastScore.Format();
            if(numberOf100s * 100 < WhoaPlayerProperties.LastScore)
            {
                numberOf100s++;
                properties.Speed++;
            }
        }

        Vector2 followerPos = Follower.position;
        followerPos.x = transform.position.x + 7;
        Vector2 difference = followerPos - (Vector2)transform.position;
        if (Mathf.Abs(difference.y) >= 9)
        {
            followerPos.y -= difference.y / 50;
        }
        Follower.position = followerPos;

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
                    bool casted = false;
                    foreach (KeyValuePair<int, GameObject> pair in selfSpellButtons)
                    {
                        if (pair.Value.collider2D.OverlapPoint(pos))
                        {
                            CastSelfSpell(WhoaPlayerProperties.Spells.SelfSpells[pair.Key]);
                            casted = true;
                            break;
                        }
                    }
                    if (!casted)
                        Flap();
                }
            }
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bool casted = false;
                foreach (KeyValuePair<int, GameObject> pair in selfSpellButtons)
                {
                    if (pair.Value.collider2D.OverlapPoint(pos))
                    {
                        CastSelfSpell(WhoaPlayerProperties.Spells.SelfSpells[pair.Key]);
                        casted = true;
                        break;
                    }
                }
                if (!casted)
                    Flap();
            }
            else
            {
                flapped = false;
                selfSpellCasted = false;
            }
        if (!bouncedOff)
        {
            rigidbody2D.AddForce(new Vector2(properties.Speed, 0), ForceMode2D.Force);
        }
    }

    public void CastSelfSpell(SelfSpell spell)
    {
        float klidCost = spell.GetKlidCost();
        if (klidCost <= properties.Klid)
        {
            audio.PlayOneShot(selfSpellCastedSound);
            properties.Klid -= klidCost;
            foreach (SelfEffect effect in spell.Effects)
            {
                StartCoroutine(effect.ApplyEffect(properties));
                StartCoroutine(AddEffectToScreen(effect));
            }
        }
    }

    public void Flap()
    {
        WhoaPlayerProperties.Character.Data.Statistics.WhoaFlaps++;
        flapped = true;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        rigidbody2D.AddForce(new Vector2(0, properties.Flap), ForceMode2D.Impulse);
        audio.PlayOneShot(flapSound);
        particles[0].Emit((int)WhoaPlayerProperties.Character.Flap / 5);
    }

    private void getRekt()
    {
        if (WhoaPlayerProperties.LastScore > WhoaPlayerProperties.HighScore)
        {
            WhoaPlayerProperties.LastWasHighscore = true;
            WhoaPlayerProperties.HighScore = WhoaPlayerProperties.LastScore;
        }
        else
            WhoaPlayerProperties.LastWasHighscore = false;
        WhoaPlayerProperties.LastMoney = (int)Mathf.Pow((WhoaPlayerProperties.LastScore / 30f), WhoaPlayerProperties.Character.Multiplier) + additionalAD;
        WhoaPlayerProperties.Character.Data.Statistics.MoneyEarned += WhoaPlayerProperties.LastMoney;
        WhoaPlayerProperties.Money += WhoaPlayerProperties.LastMoney;
        WhoaPlayerProperties.Save();
        Application.LoadLevel("Score");
    }
    /*
    public void OpenAreaSurvived(int value)
    {
        WhoaPlayerProperties.Character.Data.Statistics.OpenAreasSurvived++;
        WhoaPlayerProperties.LastScore += value;
        openAreasPassedCount++;

    }

    public void ObstaclePassed()
    {
        audio.PlayOneShot(obstaclePassedSound);
        WhoaPlayerProperties.Character.Data.Statistics.ObstaclesPassed++;
        WhoaPlayerProperties.LastScore++;
        obstaclesPassedCount++;

    }*/

    public bool CollideWith(CollisionType type)
    {
        bool result = getDamaged(properties.GetCollisionHandling(type));

        return result;
    }

    private void RefreshHealthLabel()
    {
        HealthSlider.value = properties.Health;
        HealthText.text = properties.Health.ToString();
    }

    private void RefreshKlidLabel()
    {
        KlidSlider.value = properties.Klid;
        KlidText.text = properties.Klid.ToString();
    }

    private void RefreshKlidCostLabels()
    {
        int indexCounter = 0;
        foreach (KeyValuePair<int, int> index in WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds)
        {
            SelfSpell spell = WhoaPlayerProperties.Spells.SelfSpells[index.Value];
            int klidCost = spell.GetKlidCost();
            spellKlidCostAmountsOnButtons[indexCounter].text = klidCost.FormatKlid();
            if (klidCost <= properties.Klid)
                spellKlidCostAmountsOnButtons[indexCounter].color = EnoughKlidToCastColor;
            else
                spellKlidCostAmountsOnButtons[indexCounter].color = InsufficientKlidToCastColor;
            indexCounter++;
        }
    }

    private bool getDamaged(int damage)
    {
        if (damage == -1)
            return false;

        audio.PlayOneShot(crashSound);

        properties.Health -= damage;

        if (properties.Health <= 0)
            getRekt();

        return true;
    }

    public void CollectCollectible(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.health:
                properties.Health += properties.MaxHealth / 2;
                break;
            case CollectibleType.klid:
                properties.Klid += properties.MaxKlid / 2;
                break;
            case CollectibleType.adcoin:
                additionalAD += 10;
                break;
            case CollectibleType.areaEffect:
                switch (AreaEffect.billa/*WhoaPlayerProperties.AreaEffectsProbabilities.GetRandomItem()*/)
                {
                    case AreaEffect.billa:
                        StartCoroutine(BillaEffect());
                        break;
                }
                break;
        }
    }

    private void GenerateCollectibleGeneratorTriggered(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        if (UnityEngine.Random.Range(0, 10) == 0)
        {
            GameObject prefab;

            CollectibleType type = WhoaPlayerProperties.CollectiblesProbabilities.GetRandomItem();

            switch (type)
            {
                case CollectibleType.areaEffect:
                    prefab = Collectibles.AreaEffect;
                    break;
                case CollectibleType.health:
                    prefab = Collectibles.Health;
                    break;
                case CollectibleType.adcoin:
                    prefab = Collectibles.ADCoin;
                    break;
                case CollectibleType.klid:
                    if (WhoaPlayerProperties.Character.KlidEnergy > 0)
                        prefab = Collectibles.Klid;
                    else
                        prefab = Collectibles.AreaEffect;
                    break;
                default:
                    prefab = Collectibles.AreaEffect;
                    break;
            }

            float yPosition = UnityEngine.Random.Range(-7f, 7f);
            GameObject collectible = (GameObject)Instantiate(prefab, new Vector2(rigidbody2D.position.x + 40, yPosition), new Quaternion());
            CollectibleScript script = collectible.GetComponent<CollectibleScript>();
            script.Setup(this);
        }
    }

    #region Coroutines

    public System.Collections.IEnumerator KlidRegeneration()
    {
        while (true)
        {
            properties.Klid += WhoaPlayerProperties.Character.KlidEnergyRegen;
            yield return new WaitForSeconds(1);
        }
    }

    List<bool> activeEffectsFreePositions = new List<bool>();

    public System.Collections.IEnumerator AddEffectToScreen(SelfEffect effect)
    {
        int index = activeEffectsFreePositions.IndexOf(true);
        if (index == -1)
            activeEffectsFreePositions.Add(true);
        index = activeEffectsFreePositions.IndexOf(true);
        activeEffectsFreePositions[index] = false;

        GameObject screenEffect = (GameObject)Instantiate(ActiveEffectPrefab);
        RectTransform rectTransform = screenEffect.GetComponent<RectTransform>();
        rectTransform.SetParent(ActiveEffectsContainer.transform);
        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.anchoredPosition = new Vector3(index * 90, 0);
        Text secondsText = screenEffect.GetComponentInChildren<Text>();
        Image image = screenEffect.GetComponentInChildren<Image>();
        image.sprite = effect.Sprite;
        for (int remainingSeconds = effect.Duration; remainingSeconds > 0; remainingSeconds--)
        {
            secondsText.text = remainingSeconds.ToString();
            yield return new WaitForSeconds(1);
        }
        activeEffectsFreePositions[index] = true;
        GameObject.Destroy(screenEffect);
    }

    private System.Collections.IEnumerator CameraJaggling()
    {
        int direction = 1;
        if (UnityEngine.Random.Range(0, 1) == 1)
            direction = -1;
        Camera.main.rigidbody2D.angularVelocity = 90 * direction;
        yield return new WaitForSeconds(2);
        Camera.main.rigidbody2D.angularVelocity = -90 * direction;
        yield return new WaitForSeconds(2);
        Camera.main.rigidbody2D.angularVelocity = 0;
    }

    private System.Collections.IEnumerator BillaEffect()
    {
        audio.PlayOneShot(AreaEffects.MetrixSound);
        GameObject background = (GameObject)Instantiate(AreaEffects.MetrixBackground);
        background.transform.SetParent(Follower, false);

        SpriteRenderer renderer = background.GetComponent<SpriteRenderer>();

        // FADING IN
        for (int alpha = 0; alpha < 150; alpha++)
        {
            renderer.color = new Color(255, 255, 255, alpha);
            yield return new WaitForSeconds(0.012f);
        }

        yield return new WaitForSeconds(18);

        // FADING OUT
        for (int alpha = 150; alpha > 0; alpha--)
        {
            renderer.color = new Color(255, 255, 255, alpha);
            yield return new WaitForSeconds(0.012f);
        }
        GameObject.Destroy(background);
    }
    #endregion

    [Serializable]
    public class AreaEffectsData
    {
        public GameObject MetrixBackground;
        public AudioClip MetrixSound;
    }

    [Serializable]
    public class CollectiblesPrefabs
    {
        public GameObject AreaEffect;
        public GameObject Health;
        public GameObject Klid;
        public GameObject ADCoin;
    }
}
