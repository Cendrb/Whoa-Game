using UnityEngine;
using System.Collections;

public class BordersGeneratorScript : MonoBehaviour
{
    public GameObject upperBorder;
    public GameObject lowerBorder;

    public float borderWidth;

    Vector2 lastpos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            lastpos = transform.position;
            lastpos.x += borderWidth;
            transform.position = lastpos;
            GenerateBorders();
        }
    }

    void GenerateBorders()
    {
        GameObject upper = Instantiate(upperBorder) as GameObject;
        Vector2 uPos = upperBorder.transform.position;
        uPos.x = lastpos.x;
        upper.transform.position = uPos;

        GameObject lower = Instantiate(lowerBorder) as GameObject;
        Vector2 dPos = lower.transform.position;
        dPos.x = lastpos.x;
        lower.transform.position = dPos;
    }
}
