using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public float flapAmount;
    public float speed;

    public AudioClip flapSound;
    public AudioClip deathSound;
    public AudioClip crashSound;
    public AudioClip obstaclePassedSound;
    public AudioClip startupSound;

    bool flapped = false;
    bool bouncedOff = false;
    int bouncedOffTimer = 0;
    const int bouncedOffTimerLimit = 40;

    ParticleSystem particles;

    // Use this for initialization
    private void Start()
    {
        audio.PlayOneShot(startupSound);
        WhoaPlayerProperties.Load();
        WhoaPlayerProperties.LastScore = 0;
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 velocity = rigidbody2D.velocity;
        if ((Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)) && !flapped)
        {
            velocity = Flap(velocity);
        }
        if (Input.touches.Length > 0 && !flapped)
        {
            velocity = Flap(velocity);
        }
        if (!Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.Space) && Input.touches.Length == 0)
        {
            flapped = false;
        }

        if (bouncedOff)
        {
            bouncedOffTimer++;
        }
        else
            velocity.x = speed * Time.deltaTime;
        if (bouncedOffTimer == bouncedOffTimerLimit)
        {
            bouncedOffTimer = 0;
            bouncedOff = false;
        }

        rigidbody2D.velocity = velocity;
    }

    private Vector3 Flap(Vector3 v)
    {
        flapped = true;
        v.y = flapAmount;
        audio.PlayOneShot(flapSound);
        particles.Emit(10);
        return v;
    }

    void Die()
    {
        if (WhoaPlayerProperties.LastScore > WhoaPlayerProperties.HighScore)
        {
            WhoaPlayerProperties.LastWasHighscore = true;
            WhoaPlayerProperties.HighScore = WhoaPlayerProperties.LastScore;
        }
        else
            WhoaPlayerProperties.LastWasHighscore = false;
        WhoaPlayerProperties.Money += WhoaPlayerProperties.LastScore;
        WhoaPlayerProperties.Save();
        Application.LoadLevel("WhoaScore");
    }

    public void Kill(KillerCollisionScript.KillerType type)
    {
        audio.PlayOneShot(crashSound);

        WhoaPlayerProperties.Lives--;

        if (WhoaPlayerProperties.Lives < 1)
            Die();
        else if (type == KillerCollisionScript.KillerType.obstacle)
        {
            Vector2 velocity = rigidbody2D.velocity;
            velocity.x = -(speed * Time.deltaTime);
            rigidbody2D.velocity = velocity;
            bouncedOff = true;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 7, 100, 20), WhoaPlayerProperties.LastScore.ToString());
        GUI.Label(new Rect(Screen.width - 20, 7, 100, 20), WhoaPlayerProperties.Lives.ToString());
    }

    public void ObstaclePassed()
    {
        audio.PlayOneShot(obstaclePassedSound);
        WhoaPlayerProperties.LastScore++;
        
    }
}
