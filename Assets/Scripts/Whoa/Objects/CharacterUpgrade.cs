using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CharacterUpgrade
{
    public string Name { get; private set; }
    public int MaxLevel { get; private set; }
    public List<UpgradeEffect> Effects { get; private set; }

    private int basePrice;
    private float perLevelPriceMultiplier;
    public Dictionary<string, int> levelDatabase;

    public Sprite Sprite { get; private set; }

    public CharacterUpgrade(string name, int maxLevel, int basePrice, float levelMultiplier)
    {
        Name = name;
        MaxLevel = maxLevel;
        this.basePrice = basePrice;
        this.perLevelPriceMultiplier = levelMultiplier;
        Effects = new List<UpgradeEffect>();

        Sprite = Resources.Load<Sprite>("Graphics/Upgrades/" + Name);
    }

    public int GetLevel()
    {
        return levelDatabase[Name];
    }

    public int GetPrice()
    {
        return (int)(basePrice * Mathf.Pow(perLevelPriceMultiplier, levelDatabase[Name]));
    }
}

