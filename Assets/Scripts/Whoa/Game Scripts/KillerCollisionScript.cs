using UnityEngine;
using System.Collections;

public class KillerCollisionScript : MonoBehaviour
{
    public enum CollisionType { basicObstacle, wall, njarbeitsheft3, njarbeitsheft2, njarbeitsheft1, zidan }

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
            bool result = playerScript.CollideWith(type);
            if (result && (type == CollisionType.njarbeitsheft1 || type == CollisionType.njarbeitsheft2 || type == CollisionType.njarbeitsheft3 || type == CollisionType.zidan))
                GetComponent<ExplodeScript>().Explode();
        }
    }
}
