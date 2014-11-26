using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Aspects.Self.Effects;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    public AudioClip flapSound;
    public AudioClip deathSound;
    public AudioClip crashSound;
    public AudioClip obstaclePassedSound;
    public AudioClip startupSound;

    public Text healthText;
    public Text klidText;
    public Text scoreText;
    public Text obstaclesPassed;
    public Text openAreasPassed;

    public GameObject activeEffectPrefab;
    public GameObject activeEffectsContainer;
    public GameObject castSelfSpellButtonPrefab;

    public Dictionary<int, GameObject> selfSpellButtons;

    public Canvas canvas;

    int obstaclesPassedCount;
    int openAreasPassedCount;
    PlayerDynamicProperties properties;
    bool flapped = false;
    bool selfSpellCasted = false;
    bool bouncedOff = false;
    int bouncedOffTimer = 0;
    const int bouncedOffTimerLimit = 40;
    ParticleSystem[] particles;

    // Use this for initialization
    private void Start()
    {
        audio.PlayOneShot(startupSound);
        WhoaPlayerProperties.Load();
        WhoaPlayerProperties.LastScore = 0;
        WhoaPlayerProperties.LastMoney = 0;
        properties = new PlayerDynamicProperties(WhoaPlayerProperties.Character);
        properties.HealthChanged += RefreshHealthLabel;
        properties.KlidChanged += RefreshKlidLabel;
        rigidbody2D.gravityScale = WhoaPlayerProperties.Character.Gravity;
        particles = GetComponentsInChildren<ParticleSystem>();

        foreach (KeyValuePair<int, SelfSpell> selfSpell in WhoaPlayerProperties.Spells.SelfSpells)
            selfSpell.Value.GetKlidCost(true);

        selfSpellButtons = new Dictionary<int, GameObject>();
        int counter = 130;
        foreach (KeyValuePair<int, int> index in WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds)
        {
            SelfSpell spell = WhoaPlayerProperties.Spells.SelfSpells[index.Value];
            spell.GenerateEffects();
            GameObject button = (GameObject)Instantiate(castSelfSpellButtonPrefab);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.parent = activeEffectsContainer.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector2(120, counter);
            Text abbreviateText = button.GetComponentInChildren<Text>();
            abbreviateText.text = spell.Abbreviate;
            selfSpellButtons.Add(index.Value, button);
            counter += 160;
        }

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = WhoaPlayerProperties.Character.Sprite;

        particles[0].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;
        particles[1].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;

        StartCoroutine(KlidRegeneration());

        RefreshHealthLabel();
        RefreshKlidLabel();
    }

    // Update is called once per frame
    private void Update()
    {
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
                            CastSpell(WhoaPlayerProperties.Spells.SelfSpells[pair.Key]);
                            casted = true;
                            break;
                        }
                    }
                    if (!casted)
                        Flap();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool casted = false;
            foreach (KeyValuePair<int, GameObject> pair in selfSpellButtons)
            {
                if (pair.Value.collider2D.OverlapPoint(pos))
                {
                    CastSpell(WhoaPlayerProperties.Spells.SelfSpells[pair.Key]);
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
        Vector2 velocity = rigidbody2D.velocity;
        velocity.x = properties.Speed * Time.fixedDeltaTime;
        rigidbody2D.velocity = velocity;
    }

    public void CastSpell(SelfSpell spell)
    {
        float klidCost = spell.GetKlidCost(false);
        if (klidCost <= properties.Klid)
        {
            properties.Klid -= klidCost;
            foreach (SelfEffect effect in spell.Effects)
            {
                StartCoroutine(effect.ApplyEffect(properties));
                StartCoroutine(AddEffectToScreen(effect));
            }
        }
    }

    public System.Collections.IEnumerator KlidRegeneration()
    {
        while (true)
        {
            properties.Klid += WhoaPlayerProperties.Character.KlidEnergyRegen;
            yield return new WaitForSeconds(1);
        }
    }

    int counter = 0;

    public System.Collections.IEnumerator AddEffectToScreen(SelfEffect effect)
    {
        GameObject screenEffect = (GameObject)Instantiate(activeEffectPrefab);
        RectTransform rectTransform = screenEffect.GetComponent<RectTransform>();
        rectTransform.parent = activeEffectsContainer.transform;
        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.anchoredPosition = new Vector3(counter, 0);
        counter += 90;
        Text secondsText = screenEffect.GetComponentInChildren<Text>();
        Image image = screenEffect.GetComponentInChildren<Image>();
        image.sprite = effect.Sprite;
        for (int remainingSeconds = effect.Duration; remainingSeconds > 0; remainingSeconds--)
        {
            secondsText.text = remainingSeconds.ToString();
            yield return new WaitForSeconds(1);
        }
        counter -= 90;
        GameObject.Destroy(screenEffect);
    }

    public void Flap()
    {
        Vector2 velocity = rigidbody2D.velocity;
        WhoaPlayerProperties.Character.Data.Statistics.WhoaFlaps++;
        flapped = true;
        velocity.y = properties.Flap;
        audio.PlayOneShot(flapSound);
        particles[0].Emit(10);
        rigidbody2D.velocity = velocity;
    }

    void GetRekt()
    {
        if (WhoaPlayerProperties.LastScore > WhoaPlayerProperties.HighScore)
        {
            WhoaPlayerProperties.LastWasHighscore = true;
            WhoaPlayerProperties.HighScore = WhoaPlayerProperties.LastScore;
        }
        else
            WhoaPlayerProperties.LastWasHighscore = false;
        WhoaPlayerProperties.LastMoney = (int)Mathf.Pow(WhoaPlayerProperties.LastScore, WhoaPlayerProperties.Character.Multiplier);
        WhoaPlayerProperties.Character.Data.Statistics.MoneyEarned += WhoaPlayerProperties.LastMoney;
        WhoaPlayerProperties.Money += WhoaPlayerProperties.LastMoney;
        WhoaPlayerProperties.Save();
        Application.LoadLevel("Score");
    }

    public void OpenAreaSurvived(int value)
    {
        WhoaPlayerProperties.Character.Data.Statistics.OpenAreasSurvived++;
        WhoaPlayerProperties.LastScore += value;
        openAreasPassedCount++;
        openAreasPassed.text = openAreasPassedCount.ToString();
        scoreText.text = WhoaPlayerProperties.LastScore.ToString();
    }

    public void ObstaclePassed()
    {
        audio.PlayOneShot(obstaclePassedSound);
        WhoaPlayerProperties.Character.Data.Statistics.ObstaclesPassed++;
        WhoaPlayerProperties.LastScore++;
        obstaclesPassedCount++;
        obstaclesPassed.text = obstaclesPassedCount.ToString();
        scoreText.text = WhoaPlayerProperties.LastScore.ToString();
    }

    public bool CollideWith(KillerCollisionScript.CollisionType type)
    {
        bool result = getDamaged(properties.GetCollisionHandling(type));

        if (result)
            RefreshHealthLabel();
        return result;
    }

    private void RefreshHealthLabel()
    {
        healthText.text = String.Format("{0}/{1}", properties.Health, properties.MaxHealth);
    }

    private void RefreshKlidLabel()
    {
        klidText.text = String.Format("{0}/{1}", properties.Klid, properties.MaxKlid);
    }

    private bool getDamaged(int damage)
    {
        if (damage == -1)
            return false;

        audio.PlayOneShot(crashSound);

        properties.Health -= damage;

        if (properties.Health < 1)
            GetRekt();

        return true;
    }
}
