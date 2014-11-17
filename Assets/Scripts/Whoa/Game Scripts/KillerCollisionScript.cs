using UnityEngine;
using System.Collections;

public class KillerCollisionScript : MonoBehaviour
{
    public enum CollisionType { basicObstacle, wall, frozenObstacle, slimyObstacle, njarbeitsheft3, zidan }

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
            if(type == CollisionType.njarbeitsheft3)
            {
                GetComponent<ExplosiveProjectile>().Explode();
            }
            if (type == CollisionType.zidan)
            {
                GetComponent<ExplosiveProjectile>().Explode();
            }
            playerScript.CollideWith(type);
        }
    }
}
