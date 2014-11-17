using UnityEngine;
using System.Collections;

public class FollowObjectScript : MonoBehaviour
{

    public Transform followedObjectReference;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;

        pos.x = followedObjectReference.position.x;

        transform.position = pos;
    }
}
