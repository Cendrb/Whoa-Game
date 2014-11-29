using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum CollectibleType { illuminati, health, klid, areaEffect }

public class CollectibleScript : MonoBehaviour
{
    PlayerScript script;
    CollectibleType type;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && script != null)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void Setup(PlayerScript playerScript)
    {
        script = playerScript;
        int result = UnityEngine.Random.Range(0, WhoaPlayerProperties.TotalProbability);
        type = WhoaPlayerProperties.CollectiblesProbabilities.First<KeyValuePair<CollectibleType, Range>>(new Func<KeyValuePair<CollectibleType, Range>, bool>((pair) => pair.Value.IsInRange(result))).Key;
    }
}
