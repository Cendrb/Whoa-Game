using UnityEngine;
using System.Collections;

public class ObstacleGeneratorScript : MonoBehaviour
{
    public BordersGeneratorScript bordersGeneratorScript;

    public GameObject singleObstaclePrefab;
    public GameObject obstaclePassedPrefab;
    public int offset;
    float spaceBetweenObstacles;
    int obstaclesPerSection;

    private int areasPassed = 0;

    Vector2 lastpos;

    public GameObject NJArbeitsheft;
    public GameObject Zidan;

    public PlayerScript playerScript;

    // Use this for initialization
    void Start()
    {
        spaceBetweenObstacles = WhoaPlayerProperties.Settings.SpaceBetweenObstacles;
        obstaclesPerSection = WhoaPlayerProperties.Settings.ObstaclesPerSection;
    }

    void GenerateObstaclesSet()
    {
        for (int i = 0; i < obstaclesPerSection; i++)
        {
            Vector2 pos = new Vector2();
            pos.x = lastpos.x + offset + i * obstaclesPerSection;

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
            areasPassed++;
            GenerateLevel();

        }
    }

    private void GenerateLevel()
    {
        lastpos = transform.position;
        if (areasPassed % 2 == 0)
        {
            float addition = obstaclesPerSection * spaceBetweenObstacles;
            bordersGeneratorScript.GenerateBorders(addition);
            if (areasPassed > 0)
                playerScript.OpenAreaSurvived(obstaclesPerSection + obstaclesPerSection / 2);
            GenerateObstaclesSet();
            lastpos.x += addition;
        }
        else
        {
            Debug.Log(Time.fixedDeltaTime);

            float sizeOfArea = WhoaPlayerProperties.Settings.FreeAreaSize;
            float playerTimeForFullArea = sizeOfArea / (WhoaPlayerProperties.Character.Speed * Time.fixedDeltaTime);
            float playerTimeForOffsetArea = offset / (WhoaPlayerProperties.Character.Speed * Time.fixedDeltaTime);

            float distanceMaxAdditionForZidan = playerTimeForFullArea * (WhoaPlayerProperties.Settings.ZidanSpeed) * Time.fixedDeltaTime;
            float distanceMinAdditionForZidan = playerTimeForOffsetArea * (WhoaPlayerProperties.Settings.ZidanSpeed) * Time.fixedDeltaTime;
            float distanceBetweenZidans = (distanceMaxAdditionForZidan + sizeOfArea - WhoaPlayerProperties.Settings.FreeAreaEntityOffset) / WhoaPlayerProperties.Settings.ZidanCount;

            bordersGeneratorScript.GenerateBorders(sizeOfArea + distanceMaxAdditionForZidan);
            float zidanVelocity = WhoaPlayerProperties.Settings.ZidanSpeed * Time.fixedDeltaTime;
            for (int x = WhoaPlayerProperties.Settings.ZidanCount; x != 0; x--)
                SummonZidan(new Vector2((lastpos.x + offset + (x * distanceBetweenZidans)) + distanceMinAdditionForZidan, -5), new Quaternion(), -(zidanVelocity));
            lastpos.x += sizeOfArea;
        }
        transform.position = lastpos;
    }

    private GameObject SummonNJArbeitsheft3(Vector2 pos, Quaternion quat, Vector2 velocity)
    {
        GameObject arbeitsheft = Instantiate(NJArbeitsheft, pos, quat) as GameObject;
        arbeitsheft.rigidbody2D.velocity = velocity;
        return arbeitsheft;
    }

    private GameObject SummonZidan(Vector2 pos, Quaternion quat, float velocity)
    {
        GameObject zidan = Instantiate(Zidan, pos, quat) as GameObject;
        zidan.GetComponent<ConstantVelocityScript>().xVelocity = velocity;
        return zidan;
    }
}
