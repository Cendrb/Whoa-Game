using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    public AudioClip failSound;
    public AudioClip highscoreSound;

    // Use this for initialization
    void Start()
    {
        if (WhoaPlayerProperties.LastWasHighscore)
            audio.PlayOneShot(highscoreSound);
        else
            audio.PlayOneShot(failSound);

        scoreText.text = WhoaPlayerProperties.LastScore.ToString();
        highScoreText.text = WhoaPlayerProperties.HighScore.ToString();
    }
}
