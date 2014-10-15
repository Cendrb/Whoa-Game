using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public static class WhoaCharacters
{
    public static CharactersData Characters { get; private set; }
    private static readonly string SavePath = Application.persistentDataPath + "/characters.dat";

    public static WhoaCharacters()
    {
        Load();
    }

    public static void SetupDefaults()
    {
        Characters = new CharactersData();
        Characters.SetupDefaults();
    }

    public static void Load()
    {
        if (!File.Exists(SavePath))
        {
            SetupDefaults();
            Save();
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(Application.persistentDataPath + "/characters.dat", FileMode.Open);
            Characters = (CharactersData)formatter.Deserialize(stream);
        }
    }

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + "/characters.dat", FileMode.OpenOrCreate);
        formatter.Serialize(stream, Characters);
    }

    [Serializable]
    public class CharactersData
    {
        public WhoaCharacter Andrsova { get; set; }

        public CharactersData()
        {

        }

        public void SetupDefaults()
        {
            Andrsova = new WhoaCharacter();
        }
    }
}

