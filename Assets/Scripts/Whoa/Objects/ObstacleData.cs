using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ObstacleData
{
    public float SpaceBetweenMax { get; private set; }
    public float SpaceBetweenMin { get; private set; }

    public float ObstaclesAddedPer100m { get; private set; }

    public float XVelocityMin { get; private set; }
    public float XVelocityMax { get; private set; }
    public float YVelocityMin { get; private set; }
    public float YVelocityMax { get; private set; }

    public float Offset { get; private set; }
    public int Damage { get; private set; }
    public float Mass { get; private set; }

    public ObstacleData(float spaceBetweenMin, float spaceBetweenMax, float per100mAdded, float xVelMin, float xVelMax, float yVelMin, float yVelMax, float offset, int damage, float mass)
    {
        SpaceBetweenMax = spaceBetweenMax;
        SpaceBetweenMin = spaceBetweenMin;
        ObstaclesAddedPer100m = per100mAdded;
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
        return new ObstacleData(SpaceBetweenMin, SpaceBetweenMax, ObstaclesAddedPer100m, XVelocityMin, XVelocityMax, YVelocityMin, YVelocityMax, Offset, Damage, Mass);
    }
}

