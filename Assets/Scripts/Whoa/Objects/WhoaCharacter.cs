using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public enum BuyUpgradeResult { success, insufficientMoney, maxLevelReached }
public enum BuyCharacterResult { success, insufficientMoney }

public class WhoaCharacter
{
    public float Multiplier { get; private set; }
    public int Health { get; private set; }
    public int Price { get; private set; }
    public string Name { get; private set; }
    public float Flap { get; private set; }
    public float Speed { get; private set; }
    public float Gravity { get; private set; }
    public float KlidEnergy { get; private set; }
    public float KlidEnergyRegen { get; private set; }
    public int SelfSpellSlots { get; private set; }
    public int RangedSpellSlots { get; private set; }

    private int baseHealth;
    private float baseKlidEnergy;
    private float baseKlidEnergyRegen;
    private float baseSpeed;
    private float baseFlap;
    private float baseGravity;

    public List<CharacterUpgrade> Upgrades { get; private set; }

    public Sprite Sprite { get; private set; }
    public Texture2D Texture { get; private set; }

    public WhoaCharacterData Data { get; private set; }

    private string savePath;

    private T GetDeserializedData<T>(string key, SerializationInfo info)
    {
        Type type = typeof(T);
        try
        {
            return (T)info.GetValue(key, type);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogWarning("Using default value...(" + default(T) + ")");
            return default(T);
        }
    }

    public WhoaCharacter(string name, float multiplier, int health, float flap, float speed, float gravity, float klidEnergy, float klidEnergyRegen, int selfSpellSlots, int rangedSpellSlots, int price)
    {
        Multiplier = multiplier;
        baseHealth = health;
        Name = name;
        Price = price;
        Flap = flap;
        baseFlap = flap;
        Speed = speed;
        baseSpeed = speed;
        Gravity = gravity;
        baseGravity = gravity;
        baseKlidEnergyRegen = klidEnergyRegen;
        baseKlidEnergy = klidEnergy;
        SelfSpellSlots = selfSpellSlots;
        RangedSpellSlots = rangedSpellSlots;

        Sprite = Resources.Load<Sprite>("Graphics/Characters/" + Name);

        savePath = Application.persistentDataPath + "/" + Name + ".dat";

        Upgrades = new List<CharacterUpgrade>();
    }

    public void AddUpgrade(CharacterUpgrade upgrade)
    {
        upgrade.getLevelMethod = getLevelOf;
        Upgrades.Add(upgrade);
    }

    private int getLevelOf(string upgradeName)
    {
        return Data.UpgradeLevelDatabase[upgradeName];
    }

    public void LoadEverything()
    {
        Load();
        applyUpgrades();
    }

    private void applyUpgrades()
    {
        Health = baseHealth;
        KlidEnergy = baseKlidEnergy;
        KlidEnergyRegen = baseKlidEnergyRegen;
        Gravity = baseGravity;
        Flap = baseFlap;
        Speed = baseSpeed;

        foreach (CharacterUpgrade upgrade in Upgrades)
        {
            int level = Data.UpgradeLevelDatabase[upgrade.Name];
            foreach (UpgradeEffect effect in upgrade.Effects)
            {
                switch (effect.AffectedProperty)
                {
                    case EffectAffectedProperty.health:
                        Health = (int)effect.GetModifiedValue(Health, level);
                        break;
                    case EffectAffectedProperty.klid:
                        KlidEnergy = effect.GetModifiedValue(KlidEnergy, level);
                        break;
                    case EffectAffectedProperty.klidRegen:
                        KlidEnergyRegen = effect.GetModifiedValue(KlidEnergyRegen, level);
                        break;
                    case EffectAffectedProperty.speed:
                        Speed = effect.GetModifiedValue(Speed, level);
                        break;
                    case EffectAffectedProperty.flap:
                        Flap = effect.GetModifiedValue(Flap, level);
                        break;
                    case EffectAffectedProperty.gravity:
                        Gravity = effect.GetModifiedValue(Gravity, level);
                        break;
                }
            }
        }
    }

    public void SaveDefaults()
    {
        Data = new WhoaCharacterData();
        Data.Statistics = new WhoaCharacterStatistics();
        Data.UpgradeLevelDatabase = new Dictionary<string, int>();
        Data.SelectedSelfSpellsIds = new Dictionary<int, int>();
        Data.SelectedRangedSpellsIds = new Dictionary<int, int>();
        foreach (CharacterUpgrade upgrade in Upgrades)
            Data.UpgradeLevelDatabase.Add(upgrade.Name, 0);
        Save();
    }

    public BuyUpgradeResult BuyUpgrade(CharacterUpgrade upgrade)
    {
        if (WhoaPlayerProperties.Money >= upgrade.GetPrice())
        {
            if (!Data.UpgradeLevelDatabase.ContainsKey(upgrade.Name))
                Data.UpgradeLevelDatabase.Add(upgrade.Name, 0);
            if (Data.UpgradeLevelDatabase[upgrade.Name] < upgrade.MaxLevel)
            {
                WhoaPlayerProperties.Money -= upgrade.GetPrice();
                Data.UpgradeLevelDatabase[upgrade.Name]++;
                WhoaPlayerProperties.Save();
                applyUpgrades();
                return BuyUpgradeResult.success;
            }
            else
                return BuyUpgradeResult.maxLevelReached;
        }
        else
            return BuyUpgradeResult.insufficientMoney;
    }

    public BuyCharacterResult BuyCharacter()
    {
        if (WhoaPlayerProperties.Money >= Price)
        {
            WhoaPlayerProperties.Money -= Price;
            Data.Purchased = true;
            WhoaPlayerProperties.Save();
            return BuyCharacterResult.success;
        }
        else
            return BuyCharacterResult.insufficientMoney;
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            SaveDefaults();
        }
        else
        {
            FileStream stream = File.Open(savePath, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Data = (WhoaCharacterData)formatter.Deserialize(stream);
                if (Data.SelectedRangedSpellsIds == null || Data.SelectedSelfSpellsIds == null)
                {
                    Data.SelectedRangedSpellsIds = new Dictionary<int, int>();
                    Data.SelectedSelfSpellsIds = new Dictionary<int, int>();
                    stream.Close();
                    Save();
                }
                else
                {
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                stream.Close();
                Debug.LogException(e);
                SaveDefaults();

            }
        }
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = File.Open(savePath, FileMode.OpenOrCreate))
        {
            formatter.Serialize(stream, Data);
        }
    }
}