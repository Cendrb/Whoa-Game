using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGeneratorScript : MonoBehaviour
{
    public BordersGeneratorScript bordersGeneratorScript;

    public GameObject onPlayerPassedExecutorPrefab;
    public GameObject singleObstaclePrefab;
    public GameObject obstaclePassedPrefab;

    private int areasPassed = -1;

    public GameObject NJArbeitsheft3;
    public GameObject NJArbeitsheft2;
    public GameObject NJArbeitsheft1;
    public GameObject Zidan;
    public GameObject Apple;

    public GameObject UpperBorder;
    public GameObject LowerBorder;


    List<ObstacleGameObject> DynamicObstacles = new List<ObstacleGameObject>();

    ObstacleGameObject borders;
    ObstacleGameObject obstacles;

    public PlayerScript playerScript;
    public Transform playerTransform;

    Vector2 disabledPos;

    // Use this for initialization
    void Start()
    {
        disabledPos = new Vector2(0, 300);

        Vector2 enabledPos = new Vector2(10, 0);

        DynamicObstacles.Add(new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, enabledPos + new Vector2(Random.Range(0, 30), 0), new Quaternion()), CollisionType.njarbeitsheft1));
        DynamicObstacles[0].SummonerScript.OnCollisionWithPlayer += (pos, y, script) => DynamicCollided(pos, CollisionType.njarbeitsheft1, script);

        DynamicObstacles.Add(new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, enabledPos + new Vector2(Random.Range(0, 30), 0), new Quaternion()), CollisionType.njarbeitsheft2));
        DynamicObstacles[1].SummonerScript.OnCollisionWithPlayer += (pos, y, script) => DynamicCollided(pos, CollisionType.njarbeitsheft2, script);

        DynamicObstacles.Add(new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, enabledPos + new Vector2(Random.Range(0, 30), 0), new Quaternion()), CollisionType.njarbeitsheft3));
        DynamicObstacles[2].SummonerScript.OnCollisionWithPlayer += (pos, y, script) => DynamicCollided(pos, CollisionType.njarbeitsheft3, script);

        DynamicObstacles.Add(new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, enabledPos + new Vector2(0, -5) + new Vector2(Random.Range(0, 30), 0), new Quaternion()), CollisionType.zidan));
        DynamicObstacles[3].SummonerScript.OnCollisionWithPlayer += (pos, y, script) => DynamicCollided(pos, CollisionType.zidan, script);

        DynamicObstacles.Add(new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, enabledPos + new Vector2(Random.Range(0, 30), 0), new Quaternion()), CollisionType.apple));
        DynamicObstacles[4].SummonerScript.OnCollisionWithPlayer += (pos, y, script) => DynamicCollided(pos, CollisionType.apple, script);

        borders = new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, new Vector2(2, 0), new Quaternion()), CollisionType.border);
        borders.SummonerScript.OnCollisionWithPlayer += BordersSummonerCollided;

        obstacles = new ObstacleGameObject((GameObject)Instantiate(onPlayerPassedExecutorPrefab, new Vector2(10, 0), new Quaternion()), CollisionType.basicObstacle);
        obstacles.SummonerScript.OnCollisionWithPlayer += ObstaclesSummonerCollided;
    }

    private void ObstaclesSummonerCollided(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        //GenerateObstacle(arg1);
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

    private void GenerateLevel()
    {/*
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
            int zidanCount = WhoaPlayerProperties.Settings.ZidanCount + areasPassed / 2;
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
                SummonNJArbeitsheft1(new Vector2(lastpos.x + offset + distance * sizeOfArea, 0));
            #endregion

            float freeAreaSize = WhoaPlayerProperties.Settings.FreeAreaSize;
            float offset = WhoaPlayerProperties.Settings.FreeAreaEntityOffset;

            foreach (ObstacleGameObject obstacle in DynamicObstacles)
            {
                float freeSpaceBetweenEntities = (freeAreaSize - offset) / (GetCountOf(obstacle.Type) + 1);
                obstacle.Summoner.transform.position = new Vector2(lastpos.x + offset + freeSpaceBetweenEntities, 0);
                obstacle.SummonerScript.PositionMovementAfterCollision = new Vector2(freeSpaceBetweenEntities, 0);
                obstacle.SummonerScript.ResetPassedCounter();
            }
            lastpos.x += freeAreaSize;
        }
        transform.position = lastpos;*/
    }

    private GameObject GetPrefabFor(CollisionType collisionType)
    {
        switch(collisionType)
        {
            case CollisionType.njarbeitsheft1:
                return NJArbeitsheft1;
            case CollisionType.njarbeitsheft2:
                return NJArbeitsheft2;
            case CollisionType.njarbeitsheft3:
                return NJArbeitsheft3;
            case CollisionType.zidan:
                return Zidan;
            case CollisionType.apple:
                return Apple;
            default:
                return null;
        }
    }

    private void DynamicCollided(Vector2 pos, CollisionType collisionType, OnPlayerPassedExecutorScript script)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[collisionType];
        pos += new Vector2(data.Offset, 0);
        script.PositionMovementAfterCollision = new Vector2(Random.Range(data.SpaceBetweenMin, data.SpaceBetweenMax), 0);
        GameObject dynamicObstacle = Instantiate(GetPrefabFor(collisionType), pos, new Quaternion()) as GameObject;
        dynamicObstacle.rigidbody2D.velocity = new Vector2(Random.Range(data.XVelocityMin, data.XVelocityMax), Random.Range(data.YVelocityMin, data.YVelocityMax));
        dynamicObstacle.transform.SetParent(playerTransform, true);
    }

    private void BordersSummonerCollided(Vector3 arg1, int arg2, OnPlayerPassedExecutorScript arg3)
    {
        GenerateStandardBorders(arg1);
    }

    private void GenerateStandardBorders(Vector2 pos)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[CollisionType.border];
        pos += new Vector2(data.Offset, 0);
        Instantiate(UpperBorder, pos + new Vector2(0, -9), Quaternion.Euler(0, 0, 0));
        Instantiate(LowerBorder, pos + new Vector2(0, 9), Quaternion.Euler(0, 0, 180));
    }
}
