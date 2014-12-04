using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum CollectibleType { health, klid, areaEffect }

public class CollectibleScript : MonoBehaviour
{
    public CollectibleType type;

    PlayerScript script;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && script != null)
        {
            script.CollectCollectible(type);
            GameObject.Destroy(gameObject);
        }
    }

    public void Setup(PlayerScript playerScript)
    {
        script = playerScript;
    }
}
