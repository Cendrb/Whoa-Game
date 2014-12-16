using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ObstacleData
{
    public int Count { get; private set; }
    public float ObstaclesAddedPerLevel { get; private set; }

    public float XVelocityMin { get; private set; }
    public float XVelocityMax { get; private set; }
    public float YVelocityMin { get; private set; }
    public float YVelocityMax { get; private set; }

    public float Offset { get; private set; }
    public int Damage { get; private set; }
    public float Mass { get; private set; }

    public ObstacleData(int count, float perLevel, float xVelMin, float xVelMax, float yVelMin, float yVelMax, float offset, int damage, float mass)
    {
        Count = count;
        ObstaclesAddedPerLevel = perLevel;
        XVelocityMin = xVelMin;
        XVelocityMax = xVelMax;
        YVelocityMin = yVelMin;
        YVelocityMax = yVelMax;
        Offset = offset;
        Damage = damage;
        Mass = mass;
    }

    public ObstacleData Clone()
    {
        return new ObstacleData(Count, ObstaclesAddedPerLevel, XVelocityMin, XVelocityMax, YVelocityMin, YVelocityMax, Offset, Damage, Mass);
    }
}

