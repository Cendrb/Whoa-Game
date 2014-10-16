using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class WhoaCharacters
{
    public List<WhoaCharacter> characters = new List<WhoaCharacter>();
    private readonly string SavePath = Application.persistentDataPath + "/characters.dat";

    public WhoaCharacters()
    {
        Load();
    }

    public void SetupDefaults()
    {
        characters = new List<WhoaCharacter>();
        characters.Add(new WhoaCharacter("Andršová", (float)1.12, 1, (float)23, (float)377, 0));
    }

    public void Load()
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
            characters = (List<WhoaCharacter>)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + "/characters.dat", FileMode.OpenOrCreate);
        formatter.Serialize(stream, characters);
        stream.Close();
    }

}

