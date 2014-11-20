using UnityEngine;
using System.Collections;

public class ExplodeScript : MonoBehaviour {

    public GameObject explosion;
    public int numberOfPaticles;
    public AudioClip explodeSound;


    public void Explode()
    {
        GameObject explosionGO = Instantiate(explosion, transform.position, new Quaternion()) as GameObject;
        ParticleSystem particles = explosionGO.GetComponent<ParticleSystem>();
        particles.Emit(numberOfPaticles);
        AudioSource audio = explosionGO.GetComponent<AudioSource>();
        audio.clip = explodeSound;
        audio.Play();
        GameObject.Destroy(gameObject);
    }

}
