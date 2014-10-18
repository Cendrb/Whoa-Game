﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class WhoaPlayerProperties
{
    static readonly string MONEY_PREFS_KEY = "MONEY";
    static readonly string HIGH_SCORE_PREFS_KEY = "HIGH_SCORE";

    public static WhoaCharacter Character { get; set; }
    public static WhoaCharacters Characters { get; private set; }

    public static int HighScore { get; set; }
    public static int LastScore { get; set; }
    public static int LastMoney { get; set; }
    public static int Money { get; set; }
    public static bool LastWasHighscore { get; set; }

    static WhoaPlayerProperties()
    {
        Characters = new WhoaCharacters();
        Character = Characters.characters[0];
    }

    public static void Save()
    {
        Characters.Save();
        PlayerPrefs.SetInt(MONEY_PREFS_KEY, Money);
        PlayerPrefs.SetInt(HIGH_SCORE_PREFS_KEY, HighScore);
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        Characters.Load();
        Character = Characters.characters[0];
        Money = PlayerPrefs.GetInt(MONEY_PREFS_KEY);
        HighScore = PlayerPrefs.GetInt(HIGH_SCORE_PREFS_KEY);
    }
}
