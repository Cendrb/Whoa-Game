using UnityEngine;
using System.Collections;

public class DestroyOnTouch : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject.Destroy(col.gameObject);
    }
}
