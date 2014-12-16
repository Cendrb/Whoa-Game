using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlayerDynamicProperties
{
    Dictionary<CollisionType, int> collisionHandling = new Dictionary<CollisionType, int>();
    Dictionary<CollisionType, int> defaultCollisionHandling = new Dictionary<CollisionType, int>();
    Dictionary<CollisionType, int> collisionHandlingOverrideCount = new Dictionary<CollisionType, int>();

    int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health > MaxHealth)
                health = MaxHealth;
            HealthChanged();
        }
    }
    public readonly int MaxHealth;

    float klid;
    public float Klid
    {
        get
        {
            return klid;
        }
        set
        {
            klid = value;
            if (klid > MaxKlid)
                klid = MaxKlid;
            KlidChanged();
        }
    }
    public readonly float MaxKlid;
    public float Speed { get; set; }
    public float Flap { get; set; }
    public float Gravity { get; set; }

    public event Action HealthChanged = delegate { };
    public event Action KlidChanged = delegate { };

    public PlayerDynamicProperties(WhoaCharacter character)
    {
        MaxHealth = character.Health;
        Health = character.Health;
        MaxKlid = character.KlidEnergy;
        Klid = character.KlidEnergy;
        Speed = character.Speed;
        Flap = character.Flap;
        Gravity = character.Mass;

        collisionHandling[CollisionType.basicObstacle] = 10;
        collisionHandling[CollisionType.wall] = 5;

        foreach (KeyValuePair<CollisionType, ObstacleData> pair in WhoaPlayerProperties.ObstaclesData.Data)
            collisionHandling[pair.Key] = pair.Value.Damage;

        foreach (KeyValuePair<CollisionType, int> pair in collisionHandling)
        {
            defaultCollisionHandling[pair.Key] = pair.Value;
            collisionHandlingOverrideCount[pair.Key] = 0;
        }
    }

    public void SetCollisionHandling(CollisionType type, int value)
    {
        if (collisionHandling[type] != defaultCollisionHandling[type])
            collisionHandlingOverrideCount[type]++;
        collisionHandling[type] = value;
    }

    public void RevertCollisionHandling(CollisionType type)
    {
        if (collisionHandlingOverrideCount[type] > 0)
            collisionHandlingOverrideCount[type]--;
        else
            collisionHandling[type] = defaultCollisionHandling[type];
    }

    public int GetCollisionHandling(CollisionType type)
    {
        return collisionHandling[type];
    }
}

