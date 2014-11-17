using UnityEngine;
using System.Collections;

public class ConstantVelocityScript : MonoBehaviour
{

    public float xVelocity;
    public float yVelocity;

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
    }
}
