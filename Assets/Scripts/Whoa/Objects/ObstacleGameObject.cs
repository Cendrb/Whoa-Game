using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ObstacleGameObject
{
    public GameObject Summoner { get; set; }
    public OnPlayerPassedExecutorScript SummonerScript { get; private set; }
    public CollisionType Type { get; set; }

    public ObstacleGameObject(GameObject summoner, CollisionType type)
    {
        ObstacleData data = WhoaPlayerProperties.ObstaclesData.Data[type];
        Summoner = summoner;
        SummonerScript = Summoner.GetComponent<OnPlayerPassedExecutorScript>();
        SummonerScript.PositionMovementAfterCollision = new Vector2(UnityEngine.Random.Range(data.SpaceBetweenMin, data.SpaceBetweenMax), 0);
        Type = type;
    }
}

