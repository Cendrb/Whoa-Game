using UnityEngine;
using System.Collections;
using System;

public class OnPlayerPassedExecutorScript : MonoBehaviour {

    public event Action<Vector3, int, OnPlayerPassedExecutorScript> OnCollisionWithPlayer = delegate { };
    public Vector3 PositionMovementAfterCollision { get; set; }
    int passed;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            passed++;
            transform.position = transform.position + PositionMovementAfterCollision;
            OnCollisionWithPlayer(transform.position, passed, this);
        }
    }

    public void ResetPassedCounter()
    {
        passed = 0;
    }
}
