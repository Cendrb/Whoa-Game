using UnityEngine;
using System.Collections;
using System;

public class BordersGeneratorScript : MonoBehaviour
{
    public GameObject upperBorder;
    public GameObject lowerBorder;

    public float borderWidth;

    Vector2 lastpos;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GenerateBorders();
        }
    }
    void GenerateBorders()
    {
        lastpos = transform.position;
        lastpos.x += borderWidth;
        transform.position = lastpos;

        GameObject upper = Instantiate(upperBorder) as GameObject;
        Vector2 uPos = upperBorder.transform.position;
        uPos.x = lastpos.x;
        upper.transform.position = uPos;

        GameObject lower = Instantiate(lowerBorder) as GameObject;
        Vector2 dPos = lower.transform.position;
        dPos.x = lastpos.x;
        lower.transform.position = dPos;
    }

    public void GenerateBorders(float distance)
    {
        int numberOfBorders = (int)(distance / borderWidth) + 1;
        for (int x = numberOfBorders; x > 0; x--)
            GenerateBorders();
    }
}
