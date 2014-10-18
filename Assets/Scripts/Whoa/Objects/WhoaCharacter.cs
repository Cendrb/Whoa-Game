using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[Serializable]
public class WhoaCharacter : ISerializable
{
    private readonly string MULTIPLIER = "multiplier";
    public float Multiplier { get; private set; }

    private readonly string HEALTH = "health";
    public int Health { get; private set; }

    private readonly string PURCHASED = "purchased";
    public bool Purchased { get; private set; }

    private readonly string PRICE = "price";
    public int Price { get; private set; }

    private readonly string NAME = "name";
    public string Name { get; private set; }

    private readonly string FLAP = "flap";
    public float Flap { get; private set; }

    private readonly string SPEED = "speed";
    public float Speed { get; private set; }

    private readonly string GRAVITY = "gravity";
    public float Gravity { get; private set; }

    private readonly string KLID_ENERGY = "klid_energy";
    public float KlidEnergy { get; private set; }

    private readonly string KLID_ENERGY_REGEN = "klid_energy_regen";
    public float KlidEnergyRegen { get; private set; }

    private readonly string SPELL_SLOTS = "spell_slots";
    public int SpellSlots { get; private set; }

    //// Statistics
    //private readonly string OBSTACLES_PASSED = "obstacles_passed";
    //public int ObstaclesPassed { get; set; }

    //private readonly string MONEY_EARNED = "money_earned";
    //public int MoneyEarned { get; set; }

    //private readonly string WHOA_FLAPS = "whoa_flaps";
    //public int WhoaFlaps { get; set; }

    //Deserialization constructor.
    public WhoaCharacter(SerializationInfo info, StreamingContext ctxt)
    {
        Multiplier = GetDeserializedData<float>(MULTIPLIER, info);
        Health = GetDeserializedData<int>(HEALTH, info);
        Purchased = GetDeserializedData<bool>(PURCHASED, info);
        Price = GetDeserializedData<int>(PRICE, info);
        Name = GetDeserializedData<string>(NAME, info);
        Flap = GetDeserializedData<float>(FLAP, info);
        Speed = GetDeserializedData<float>(SPEED, info);
        Gravity = GetDeserializedData<float>(GRAVITY, info);
        //ObstaclesPassed = GetDeserializedData<int>(OBSTACLES_PASSED, info);
        //MoneyEarned = GetDeserializedData<int>(MONEY_EARNED, info);
        //WhoaFlaps = GetDeserializedData<int>(WHOA_FLAPS, info);
        KlidEnergy = GetDeserializedData<float>(KLID_ENERGY, info);
        KlidEnergyRegen = GetDeserializedData<float>(KLID_ENERGY_REGEN, info);
        SpellSlots = GetDeserializedData<int>(SPELL_SLOTS, info);
    }

    private T GetDeserializedData<T>(string key, SerializationInfo info)
    {
        Type type = typeof(T);
        try
        {
            return (T)info.GetValue(key, type);
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            Debug.LogWarning("Using default value...(" + default(T) + ")");
            return default(T);
        }
    }

    //Manual creation constructor
    public WhoaCharacter(string name, float multiplier, int lives, float flap, float speed, float gravity, float klidEnergy, float klidEnergyRegen, int spellSlotsCount, int price)
    {
        Multiplier = multiplier;
        Health = lives;
        Name = name;
        Price = price;
        Flap = flap;
        Speed = speed;
        Gravity = gravity;
        KlidEnergyRegen = klidEnergyRegen;
        KlidEnergy = klidEnergy;
        SpellSlots = spellSlotsCount;
    }

    //Serialization function.
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue(MULTIPLIER, Multiplier);
        info.AddValue(HEALTH, Health);
        info.AddValue(PURCHASED, Purchased);
        info.AddValue(PRICE, Price);
        info.AddValue(NAME, Name);
        info.AddValue(SPEED, Speed);
        info.AddValue(FLAP, Flap);
        info.AddValue(GRAVITY, Gravity);
        info.AddValue(OBSTACLES_PASSED, ObstaclesPassed);
        info.AddValue(MONEY_EARNED, MoneyEarned);
        info.AddValue(WHOA_FLAPS, WhoaFlaps);
        info.AddValue(KLID_ENERGY, KlidEnergy);
        info.AddValue(KLID_ENERGY_REGEN, KlidEnergyRegen);
        info.AddValue(SPELL_SLOTS, SpellSlots);
    }
}