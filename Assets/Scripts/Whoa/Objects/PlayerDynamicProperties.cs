using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlayerDynamicProperties
{
    Dictionary<KillerCollisionScript.CollisionType, int> collisionHandling = new Dictionary<KillerCollisionScript.CollisionType, int>();
    Dictionary<KillerCollisionScript.CollisionType, int> defaultCollisionHandling = new Dictionary<KillerCollisionScript.CollisionType, int>();

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

        collisionHandling[KillerCollisionScript.CollisionType.basicObstacle] = 10;
        collisionHandling[KillerCollisionScript.CollisionType.wall] = 5;
        collisionHandling[KillerCollisionScript.CollisionType.zidan] = 20;
        collisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft1] = 3;
        collisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft2] = 5;
        collisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft3] = 7;

        defaultCollisionHandling[KillerCollisionScript.CollisionType.basicObstacle] = collisionHandling[KillerCollisionScript.CollisionType.basicObstacle];
        defaultCollisionHandling[KillerCollisionScript.CollisionType.wall] = collisionHandling[KillerCollisionScript.CollisionType.wall];
        defaultCollisionHandling[KillerCollisionScript.CollisionType.zidan] = collisionHandling[KillerCollisionScript.CollisionType.zidan];
        defaultCollisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft1] = collisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft1];
        defaultCollisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft2] = collisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft2];
        defaultCollisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft3] = collisionHandling[KillerCollisionScript.CollisionType.njarbeitsheft3];
    }

    public void SetCollisionHandling(KillerCollisionScript.CollisionType type, int value)
    {
        collisionHandling[type] = value;
    }

    public void RevertCollisionHandling(KillerCollisionScript.CollisionType type)
    {
        collisionHandling[type] = defaultCollisionHandling[type];
    }

    public int GetCollisionHandling(KillerCollisionScript.CollisionType type)
    {
        return collisionHandling[type];
    }
}

