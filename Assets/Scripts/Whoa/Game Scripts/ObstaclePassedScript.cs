using UnityEngine;
using System.Collections;

public class ObstaclePassedScript : MonoBehaviour
{
    PlayerScript playerScript;

    bool passed = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!passed && col.CompareTag("Player"))
        {
            passed = true;
            playerScript.ObstaclePassed();
        }
    }
}
