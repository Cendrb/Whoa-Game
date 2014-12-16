using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ObstacleGameObject
{
    public GameObject Prefab { get; set; }
    public GameObject Summoner { get; set; }
    public OnPlayerPassedExecutorScript SummonerScript { get; private set; }
    public CollisionType Type { get; set; }

    public ObstacleGameObject(GameObject prefab, GameObject summoner, CollisionType type)
    {
        Prefab = prefab;
        Summoner = summoner;
        SummonerScript = Summoner.GetComponent<OnPlayerPassedExecutorScript>();
        Type = type;
    }
}

