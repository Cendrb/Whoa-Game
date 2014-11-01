using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class WhoaPlayerProperties
{
    static readonly string MONEY_PREFS_KEY = "MONEY";
    static readonly string HIGH_SCORE_PREFS_KEY = "HIGH_SCORE";
    static readonly string SELECTED_CHARACTER_INDEX = "SELECTED_CHARACTER_INDEX";

    public static WhoaCharacter Character { get; private set; }
    public static WhoaCharacters Characters { get; private set; }

    public static int HighScore { get; set; }
    public static int LastScore { get; set; }
    public static int LastMoney { get; set; }
    public static int Money { get; set; }
    public static bool LastWasHighscore { get; set; }

    private static int selectedCharacterIndex;

    static WhoaPlayerProperties()
    {
        Characters = new WhoaCharacters();
        Load();
    }

    public static void SetCharacter(WhoaCharacter character)
    {
        Character = character;
        selectedCharacterIndex = Characters.characters.IndexOf(character);
        SaveWithoutCharacters();
    }

    public static void SetCharacter(int characterIndex)
    {
        Character = Characters.characters[characterIndex];
        selectedCharacterIndex = characterIndex;
        SaveWithoutCharacters();
    }

    public static void SaveWithoutCharacters()
    {
        PlayerPrefs.SetInt(MONEY_PREFS_KEY, Money);
        PlayerPrefs.SetInt(HIGH_SCORE_PREFS_KEY, HighScore);
        PlayerPrefs.SetInt(SELECTED_CHARACTER_INDEX, selectedCharacterIndex);
        PlayerPrefs.Save();
    }

    public static void LoadWithoutCharacters()
    {
        Money = PlayerPrefs.GetInt(MONEY_PREFS_KEY);
        HighScore = PlayerPrefs.GetInt(HIGH_SCORE_PREFS_KEY);
        SetCharacter(PlayerPrefs.GetInt(SELECTED_CHARACTER_INDEX));
    }

    public static void Save()
    {
        Characters.Save();
        SaveWithoutCharacters();
    }

    public static void Load()
    {
        Characters.Load();
        LoadWithoutCharacters();
    }
}
