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

    private readonly string LIVES = "lives";
    public int Lives { get; private set; }

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

    //Deserialization constructor.
    public WhoaCharacter(SerializationInfo info, StreamingContext ctxt)
    {
        Multiplier = (float)info.GetValue(MULTIPLIER, typeof(float));
        Lives = (int)info.GetValue(LIVES, typeof(int));
        Purchased = (bool)info.GetValue(PURCHASED, typeof(bool));
        Price = (int)info.GetValue(PRICE, typeof(int));
        Name = (string)info.GetValue(NAME, typeof(string));
        Flap = (float)info.GetValue(FLAP, typeof(float));
        Speed = (float)info.GetValue(SPEED, typeof(float));
        Gravity = (float)info.GetValue(GRAVITY, typeof(float));
    }

    //Manual creation constructor
    public WhoaCharacter(string name, float multiplier, int lives, float flap, float speed, float gravity, int price)
    {
        Multiplier = multiplier;
        Lives = lives;
        Name = name;
        Price = price;
        Flap = flap;
        Speed = speed;
        Gravity = gravity;
    }

    //Serialization function.
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue(MULTIPLIER, Multiplier);
        info.AddValue(LIVES, Lives);
        info.AddValue(PURCHASED, Purchased);
        info.AddValue(PRICE, Price);
        info.AddValue(NAME, Name);
        info.AddValue(SPEED, Speed);
        info.AddValue(FLAP, Flap);
        info.AddValue(GRAVITY, Gravity);
    }
}