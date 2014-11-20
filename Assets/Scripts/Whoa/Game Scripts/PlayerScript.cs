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

    bool flapped = false;
    bool bouncedOff = false;
    int bouncedOffTimer = 0;
    int health;
    float speed;
    float flap;
    const int bouncedOffTimerLimit = 40;

    ParticleSystem[] particles;

    // Use this for initialization
    private void Start()
    {
        audio.PlayOneShot(startupSound);
        WhoaPlayerProperties.Load();
        WhoaPlayerProperties.LastScore = 0;
        WhoaPlayerProperties.LastMoney = 0;
        health = WhoaPlayerProperties.Character.Health;
        speed = WhoaPlayerProperties.Character.Speed;
        flap = WhoaPlayerProperties.Character.Flap;
        rigidbody2D.gravityScale = WhoaPlayerProperties.Character.Gravity;
        particles = GetComponentsInChildren<ParticleSystem>();

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = WhoaPlayerProperties.Character.Sprite;

        particles[0].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;
        particles[1].renderer.material.mainTexture = WhoaPlayerProperties.Character.Sprite.texture;

        healthText.text = String.Format("{0}/{1}", health, WhoaPlayerProperties.Character.Health);
        // TODO Whoa! Spells!
        klidText.text = String.Format("{0}/{1}", WhoaPlayerProperties.Character.KlidEnergy, WhoaPlayerProperties.Character.KlidEnergy);
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
        velocity.y = flap;
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
        if (WhoaPlayerProperties.LastScore % 7 == 0)
            speed++;
        obstaclesPassedCount++;
        obstaclesPassed.text = obstaclesPassedCount.ToString();
        scoreText.text = WhoaPlayerProperties.LastScore.ToString();
    }

    public bool CollideWith(KillerCollisionScript.CollisionType type)
    {
        switch(type)
        {
            case KillerCollisionScript.CollisionType.basicObstacle:
                getDamaged(10);
                return true;
            case KillerCollisionScript.CollisionType.wall:
                getDamaged(5);
                return true;
            case KillerCollisionScript.CollisionType.njarbeitsheft3:
                getDamaged(7);
                return true;
            case KillerCollisionScript.CollisionType.njarbeitsheft2:
                getDamaged(5);
                return true;
            case KillerCollisionScript.CollisionType.njarbeitsheft1:
                getDamaged(3);
                return true;
            case KillerCollisionScript.CollisionType.zidan:
                getDamaged(20);
                return true;
        }

        healthText.text = String.Format("{0}/{1}", health, WhoaPlayerProperties.Character.Health);
        return false;
    }

    private void getDamaged(int damage)
    {
        audio.PlayOneShot(crashSound);

        health -= damage;

        if (health < 1)
            GetRekt();
    }
}
