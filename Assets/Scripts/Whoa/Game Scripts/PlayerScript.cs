using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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

    int obstaclesPassedCount;
    int openAreasPassedCount;

    PlayerDynamicProperties properties;

    bool flapped = false;
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

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = WhoaPlayerProperties.Character.Sprite;

        particles[0].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;
        particles[1].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;

        RefreshHealthLabel();
        RefreshKlidLabel();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            if (!flapped)
            {
                Flap();
                flapped = true;
            }
        }
        else
            flapped = false;/*
        Vector2 velocity = rigidbody2D.velocity;
        if (bouncedOff)
        {
            bouncedOffTimer++;
        }
        else
            velocity.x = speed * Time.fixedDeltaTime;
        if (bouncedOffTimer == bouncedOffTimerLimit)
        {
            bouncedOffTimer = 0;
            bouncedOff = false;
        }

        rigidbody2D.velocity = velocity;*/
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
