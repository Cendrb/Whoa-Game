using UnityEngine;
using System.Collections;

public class ExplosiveProjectile : MonoBehaviour {

    public GameObject explosion;

    public void Explode()
    {
        Instantiate(explosion, transform.position, new Quaternion());
        GameObject.Destroy(gameObject);
    }
}
