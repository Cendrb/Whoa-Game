﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
    public Text klidText;
    public Text highScoreText;
    public Text scoreText;
    public AudioClip failSound;
    public AudioClip highscoreSound;

    // Use this for initialization
    void Start()
    {
        if (WhoaPlayerProperties.LastWasHighscore)
            GetComponent<AudioSource>().PlayOneShot(highscoreSound);
        else
            GetComponent<AudioSource>().PlayOneShot(failSound);

        klidText.text = WhoaPlayerProperties.LastMoney.ToString();
        scoreText.text = WhoaPlayerProperties.LastScore.ToString();
        highScoreText.text = WhoaPlayerProperties.HighScore.ToString();
    }
}
