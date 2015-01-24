using Aspects;
using Aspects.Self;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public enum AreaEffect { smokeWeedEveryday, illuminati, ADZone, school, billa, parking, superstars }

public static class WhoaPlayerProperties
{
    static readonly string MONEY_PREFS_KEY = "MONEY";
    static readonly string HIGH_SCORE_PREFS_KEY = "HIGH_SCORE";
    static readonly string SELECTED_CHARACTER_INDEX = "SELECTED_CHARACTER_INDEX";
    static readonly string LAST_SELF_SPELL_ID = "LAST_SELF_SPELL_ID";
    static readonly string LAST_RANGED_SPELL_ID = "LAST_RANGED_SPELL_ID";

    public static readonly string DRIVE_DOCUMENT_URL = "14Xi5jjCzV7BFX1cNz4ezisGnQI7qgNtTCnV3kWbMiGs";

    public static WhoaCharacter Character { get; private set; }
    public static WhoaCharacters Characters { get; private set; }

    public static GameSettings Settings { get; private set; }
    public static AspectsTemplatesStorage AspectsTemplates { get; private set; }
    public static SpellManager Spells { get; private set; }

    public static ObstaclesData ObstaclesData { get; private set; }

    public static RandomListWithChances<CollectibleType> CollectiblesProbabilities { get; private set; }
    public static RandomListWithChances<int> CaveBorderAnglesProbabilities { get; private set; }
    public static RandomListWithChances<AreaEffect> AreaEffectsProbabilities { get; private set; }

    public static int HighScore { get; set; }
    public static int LastScore { get; set; }
    public static int LastMoney { get; set; }
    public static int Money { get; set; }
    public static bool LastWasHighscore { get; set; }

    public static string NumberFormat = "N";
    public static CultureInfo Culture;

    private static int selectedCharacterIndex;

    static WhoaPlayerProperties()
    {
        ObstaclesData = new global::ObstaclesData();

        CollectiblesProbabilities = new RandomListWithChances<CollectibleType>();
        CollectiblesProbabilities.AddItem(CollectibleType.areaEffect, 10);
        CollectiblesProbabilities.AddItem(CollectibleType.adcoin, 18);
        CollectiblesProbabilities.AddItem(CollectibleType.health, 2);
        CollectiblesProbabilities.AddItem(CollectibleType.klid, 2);

        CaveBorderAnglesProbabilities = new RandomListWithChances<int>();
        CaveBorderAnglesProbabilities.AddItem(0, 1);
        CaveBorderAnglesProbabilities.AddItem(45, 1);
        CaveBorderAnglesProbabilities.AddItem(-45, 1);

        AreaEffectsProbabilities = new RandomListWithChances<AreaEffect>();
        //AreaEffectsProbabilities.AddItem(AreaEffect.billa, 1);
        AreaEffectsProbabilities.AddItem(AreaEffect.superstars, 1);

        Culture = new CultureInfo("cs-CZ");
        Characters = new WhoaCharacters();
        Settings = new GameSettings();
        Settings.MinimalCollectiblesDistance = 20;
        Spells = new SpellManager();
        AspectsTemplates = new AspectsTemplatesStorage();

        WhoaPlayerProperties.ObstaclesData.Data.Add(CollisionType.border, new ObstacleData(30, 30, 0, 0, 0, 0, 0, 100, 5, float.MaxValue));
        WhoaPlayerProperties.ObstaclesData.Data.Add(CollisionType.basicObstacle, new ObstacleData(30, 30, 0, 0, 0, 0, 0, 20, 5, float.MaxValue));

        Load();
    }

    public static void SetCharacter(WhoaCharacter character)
    {
        Character = character;
        selectedCharacterIndex = Characters.characters.IndexOf(character);
        SavePrefs();
    }
    public static void SetCharacter(int characterIndex)
    {
        Character = Characters.characters[characterIndex];
        selectedCharacterIndex = characterIndex;
        SavePrefs();
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetInt(MONEY_PREFS_KEY, Money);
        PlayerPrefs.SetInt(HIGH_SCORE_PREFS_KEY, HighScore);
        PlayerPrefs.SetInt(SELECTED_CHARACTER_INDEX, selectedCharacterIndex);
        PlayerPrefs.SetInt(LAST_SELF_SPELL_ID, Spells.SelfSpellIdCounter);
        PlayerPrefs.SetInt(LAST_RANGED_SPELL_ID, Spells.RangedSpellIdCounter);
        PlayerPrefs.Save();
    }
    public static void LoadPrefs()
    {
        Money = PlayerPrefs.GetInt(MONEY_PREFS_KEY);
        HighScore = PlayerPrefs.GetInt(HIGH_SCORE_PREFS_KEY);
        Spells.SelfSpellIdCounter = PlayerPrefs.GetInt(LAST_SELF_SPELL_ID);
        Spells.RangedSpellIdCounter = PlayerPrefs.GetInt(LAST_RANGED_SPELL_ID);
        SetCharacter(PlayerPrefs.GetInt(SELECTED_CHARACTER_INDEX));
    }

    public static void Save()
    {
        Characters.Save();
        SavePrefs();
        Spells.SaveSpells();
        AspectsTemplates.Save();
    }
    public static void Load()
    {
        Characters.Load();
        LoadPrefs();
        Spells.LoadSpells();
        AspectsTemplates.Load();
    }

    public static void ReloadFromDrive()
    {
        Characters.SetupCharacters();
        AspectsTemplates = new AspectsTemplatesStorage();
    }

    public static string Format(this int source)
    {
        return source.ToString("N0", Culture);
    }

    public static string FormatAD(this int source)
    {
        return source.ToString("N0", Culture) + " ∅";
    }

    public static string FormatKlid(this int source)
    {
        return source.ToString("N0", Culture) + " K";
    }
}
