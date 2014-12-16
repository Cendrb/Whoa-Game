using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGeneratorScript : MonoBehaviour
{
    public BordersGeneratorScript bordersGeneratorScript;

    public GameObject onPlayerPassedExecutorPrefab;
    public GameObject singleObstaclePrefab;
    public GameObject obstaclePassedPrefab;
    public int offset;

    int arbeitsheft1Count;
    int arbeitsheft2Count;
    int arbeitsheft3Count;
    int zidanCount;
    float spaceBetweenObstacles;
    int obstaclesPerSection;

    private int areasPassed = -1;

    Vector2 lastpos;

    public GameObject NJArbeitsheft3;
    public GameObject NJArbeitsheft2;
    public GameObject NJArbeitsheft1;
    public GameObject Zidan;

    List<ObstacleGameObject> Obstacles = new List<ObstacleGameObject>();

    public PlayerScript playerScript;
    public Transform playerTransform;

    Vector2 disabledPos;

    // Use this for initialization
    void Start()
    {
        spaceBetweenObstacles = WhoaPlayerProperties.Settings.SpaceBetweenObstacles;
        obstaclesPerSection = WhoaPlayerProperties.Settings.ObstaclesPerSection;

        disabledPos = new Vector2(0, 20);

        Obstacles.Add(new ObstacleGameObject(NJArbeitsheft1, (GameObject)Instantiate(onPlayerPassedExecutorPrefab, disabledPos, new Quaternion()), CollisionType.njarbeitsheft1));
        Obstacles[0].SummonerScript.OnCollisionWithPlayer += NJArbeitsheft1SummonerCollided;

        Obstacles.Add(new ObstacleGameObject(NJArbeitsheft2, (GameObject)Instantiate(onPlayerPassedExecutorPrefab, disabledPos, new Quaternion()), CollisionType.njarbeitsheft2));
        Obstacles[1].SummonerScript.OnCollisionWithPlayer += NJArbeitsheft2SummonerCollided;

        Obstacles.Add(new ObstacleGameObject(NJArbeitsheft3, (GameObject)Instantiate(onPlayerPassedExecutorPrefab, disabledPos, new Quaternion()), CollisionType.njarbeitsheft3));
        Obstacles[2].SummonerScript.OnCollisionWithPlayer += NJArbeitsheft3SummonerCollided;

        Obstacles.Add(new ObstacleGameObject(Zidan, (GameObject)Instantiate(onPlayerPassedExecutorPrefab, disabledPos, new Quaternion()), CollisionType.zidan));
        Obstacles[3].SummonerScript.OnCollisionWithPlayer += ZidanSummonerCollided;
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
            #region Old entity summoner
            /*int zidanCount = WhoaPlayerProperties.Settings.ZidanCount + areasPassed / 2;
            float minimumArbeitsheftDistance = 0.001f;
            float maximumArbeitsheftDistance = 1.8f;

            float sizeOfArea = WhoaPlayerProperties.Settings.FreeAreaSize;
            float playerTimeForFullArea = sizeOfArea / (WhoaPlayerProperties.Character.Speed * Time.fixedDeltaTime);
            float playerTimeForOffsetArea = offset / (WhoaPlayerProperties.Character.Speed * Time.fixedDeltaTime);

            float distanceMaxAdditionForZidan = playerTimeForFullArea * (WhoaPlayerProperties.Settings.ZidanSpeed) * Time.fixedDeltaTime;
            float distanceMinAdditionForZidan = playerTimeForOffsetArea * (WhoaPlayerProperties.Settings.ZidanSpeed) * Time.fixedDeltaTime;
            float distanceBetweenZidans = (distanceMaxAdditionForZidan + sizeOfArea - WhoaPlayerProperties.Settings.FreeAreaEntityOffset) / zidanCount;

            bordersGeneratorScript.GenerateBorders(sizeOfArea + distanceMaxAdditionForZidan);
            float zidanVelocity = WhoaPlayerProperties.Settings.ZidanSpeed * Time.fixedDeltaTime;
            for (int x = zidanCount; x != 0; x--)
                SummonZidan(new Vector2((lastpos.x + offset + (x * distanceBetweenZidans)) + distanceMinAdditionForZidan, -5), new Quaternion(), new Vector2(-(zidanVelocity), 0));
            lastpos.x += sizeOfArea;

            for (float distance = minimumArbeitsheftDistance; distance < maximumArbeitsheftDistance; distance += 0.6f)
                SummonNJArbeitsheft3(new Vector2(lastpos.x + offset + distance * sizeOfArea, 0));

            for (float distance = minimumArbeitsheftDistance; distance < maximumArbeitsheftDistance; distance += 0.5f)
                SummonNJArbeitsheft2(new Vector2(lastpos.x + offset + distance * sizeOfArea, 0));

            for (float distance = minimumArbeitsheftDistance; distance < maximumArbeitsheftDistance; distance += 0.4f)
                SummonNJArbeitsheft1(new Vector2(lastpos.x + offset + distance * sizeOfArea, 0));*/
            #endregion

            float freeAreaSize = WhoaPlayerProperties.Settings.FreeAreaSize;
            float offset = WhoaPlayerProperties.Settings.FreeAreaEntityOffset;

            foreach (ObstacleGameObject obstacle in Obstacles)
            {
                float freeSpaceBetweenEntities = (freeAreaSize - offset) / (GetCountOf(obstacle.Type) + 1);
                obstacle.Summoner.transform.position = new Vector2(lastpos.x + offset + freeSpaceBetweenEntities, 0);
                obstacle.SummonerScript.PositionMovementAfterCollision = new Vector2(freeSpaceBetweenEntities, 0);
                obstacle.SummonerScript.ResetPassedCounter();
            }
            lastpos.x += freeAreaSize;
        }
        transform.position = lastpos;
    }

    private void ZidanSummonerCollided(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        if (arg2 >= GetCountOf(CollisionType.zidan))
            arg3.gameObject.transform.position = disabledPos;

        SummonZidan(arg1);
    }

    private void NJArbeitsheft3SummonerCollided(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        if (arg2 >= GetCountOf(CollisionType.njarbeitsheft3))
            arg3.gameObject.transform.position = disabledPos;

        SummonNJArbeitsheft3(arg1);
    }

    private void NJArbeitsheft2SummonerCollided(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        if (arg2 >= GetCountOf(CollisionType.njarbeitsheft2))
            arg3.gameObject.transform.position = disabledPos;

        SummonNJArbeitsheft2(arg1);
    }

    private void NJArbeitsheft1SummonerCollided(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        if (arg2 >= GetCountOf(CollisionType.njarbeitsheft1))
            arg3.gameObject.transform.position = disabledPos;

        SummonNJArbeitsheft1(arg1);
    }

    private int GetCountOf(CollisionType obstacleType)
    {
        return (int)(WhoaPlayerProperties.ObstaclesData.Data[obstacleType].Count + (WhoaPlayerProperties.ObstaclesData.Data[obstacleType].ObstaclesAddedPerLevel * (areasPassed / 2)));
    }

    private GameObject SummonNJArbeitsheft3(Vector2 pos)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[CollisionType.njarbeitsheft3];
        pos += new Vector2(data.Offset, 0);
        GameObject arbeitsheft = Instantiate(NJArbeitsheft3, pos, new Quaternion()) as GameObject;
        arbeitsheft.rigidbody2D.velocity = new Vector2(Random.Range(data.XVelocityMin, data.XVelocityMax), Random.Range(data.YVelocityMin, data.YVelocityMax));
        arbeitsheft.transform.SetParent(playerTransform, true);
        return arbeitsheft;
    }

    private GameObject SummonNJArbeitsheft2(Vector2 pos)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[CollisionType.njarbeitsheft2];
        pos += new Vector2(data.Offset, 0);
        GameObject arbeitsheft = Instantiate(NJArbeitsheft2, pos, new Quaternion()) as GameObject;
        arbeitsheft.rigidbody2D.velocity = new Vector2(Random.Range(data.XVelocityMin, data.XVelocityMax), Random.Range(data.YVelocityMin, data.YVelocityMax));
        arbeitsheft.transform.SetParent(playerTransform, true);
        return arbeitsheft;
    }

    private GameObject SummonNJArbeitsheft1(Vector2 pos)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[CollisionType.njarbeitsheft1];
        pos += new Vector2(data.Offset, 0);
        GameObject arbeitsheft = Instantiate(NJArbeitsheft1, pos, new Quaternion()) as GameObject;

        arbeitsheft.rigidbody2D.velocity = new Vector2(Random.Range(data.XVelocityMin, data.XVelocityMax), Random.Range(data.YVelocityMin, data.YVelocityMax));
        arbeitsheft.transform.SetParent(playerTransform, true);
        return arbeitsheft;
    }

    private GameObject SummonZidan(Vector2 pos)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[CollisionType.zidan];
        pos += new Vector2(data.Offset, 0);
        pos = new Vector2(pos.x, -5);
        GameObject zidan = Instantiate(Zidan, pos, new Quaternion()) as GameObject;

        zidan.rigidbody2D.velocity = new Vector2(Random.Range(data.XVelocityMin, data.XVelocityMax), Random.Range(data.YVelocityMin, data.YVelocityMax));
        zidan.transform.SetParent(playerTransform, true);
        return zidan;
    }
}
