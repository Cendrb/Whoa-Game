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
        characters.Add(new WhoaCharacter("Andršová", (float)1.12, 5, (float)23, (float)377, (float)6, 200, (float)0.01, 1, 69));
    }

    public void Load()
    {
        Debug.Log("Loading characters...");
        if (!File.Exists(SavePath))
        {
            SetupDefaults();
            Save();
        }
        else
        {
            FileStream stream = File.Open(Application.persistentDataPath + "/characters.dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                characters = (List<WhoaCharacter>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch(Exception e)
            {
                stream.Close();
                SetupDefaults();
                Save();
            }
        }
    }

    public void Save()
    {
        Debug.Log("Saving characters...");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + "/characters.dat", FileMode.OpenOrCreate);
        formatter.Serialize(stream, characters);
        stream.Close();
    }

}

