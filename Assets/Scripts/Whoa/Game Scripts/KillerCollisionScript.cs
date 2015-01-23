using UnityEngine;
using System.Collections;

public enum CollisionType { basicObstacle, border, njarbeitsheft3, njarbeitsheft2, njarbeitsheft1, zidan, apple }

public class KillerCollisionScript : MonoBehaviour
{
    public CollisionType type;
    PlayerScript playerScript;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            if (playerScript == null)
                playerScript = col.gameObject.GetComponent<PlayerScript>();
            bool result = playerScript.CollideWith(type);
            if (result && (type == CollisionType.njarbeitsheft1 || type == CollisionType.njarbeitsheft2 || type == CollisionType.njarbeitsheft3 || type == CollisionType.zidan || type == CollisionType.apple))
                GetComponent<ExplodeScript>().Explode();
        }
    }
}
