using UnityEngine;
using System.Collections;

public class KillerCollisionScript : MonoBehaviour
{
    public enum CollisionType { basicObstacle, wall, frozenObstacle, slimyObstacle }

    public CollisionType type;
    PlayerScript playerScript;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerScript.CollideWith(type);
        }
    }
}
