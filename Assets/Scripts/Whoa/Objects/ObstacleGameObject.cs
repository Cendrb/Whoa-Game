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
        Summoner = summoner;
        SummonerScript = Summoner.GetComponent<OnPlayerPassedExecutorScript>();
        Type = type;
    }
}

