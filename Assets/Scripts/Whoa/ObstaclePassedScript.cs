using UnityEngine;
using System.Collections;

public class ObstaclePassedScript : MonoBehaviour
{
    PlayerScript playerScript;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision!");
        if (col.CompareTag("Player"))
        {
            playerScript.ObstaclePassed();
        }
    }
}
