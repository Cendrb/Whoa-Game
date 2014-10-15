using UnityEngine;
using System.Collections;

public class ObstacleGeneratorScript : MonoBehaviour
{
    public GameObject singleObstaclePrefab;
    public GameObject obstaclePassedPrefab;
    public float space;
    public int count;
    public int offset;
    Vector2 lastpos;

    // Use this for initialization
    void Start()
    {
    }

    void GenerateObstaclesSet()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2();
            pos.x = lastpos.x + offset + i * space;

            pos.y = Random.Range(-5, 5);

            pos.y -= 9;

            GenerateObstacle(pos);

            pos.y += 18;

            GenerateObstacle(pos);

            pos.y = 0;

            GenerateObstaclePassedDetector(pos);
        }
    }

    void GenerateObstacle(Vector2 pos)
    {
        GameObject obstacle = Instantiate(singleObstaclePrefab, pos, Quaternion.identity) as GameObject;

        Quaternion rotation = obstacle.transform.rotation;
        Vector2 theScale = obstacle.transform.localScale;

        if (pos.y < 0)
            rotation.z = 180;

        theScale.y = Random.Range(1.4F, 1.8F);

        obstacle.transform.rotation = rotation;
        obstacle.transform.localScale = theScale;
    }

    void GenerateObstaclePassedDetector(Vector2 pos)
    {
        Instantiate(obstaclePassedPrefab, pos, new Quaternion());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            lastpos = transform.position;
            GenerateObstaclesSet();
            float addition = count * space;
            lastpos.x += addition;
            transform.position = lastpos;
        }
    }
}
