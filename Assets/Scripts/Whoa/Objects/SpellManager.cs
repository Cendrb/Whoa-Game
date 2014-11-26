using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;


public class SpellManager
{
    readonly string SELF_SPELLS_PATH = Application.persistentDataPath + "/" + "selfspells.dat";
    readonly string RANGED_SPELLS_PATH = Application.persistentDataPath + "/" + "rangedspells.dat";

    public int SelfSpellIdCounter { get; set; }
    public int RangedSpellIdCounter { get; set; }

    public Dictionary<int, SelfSpell> SelfSpells { get; private set; }

    public void AddSelfSpell(SelfSpell spell)
    {
        SelfSpells.Add(SelfSpellIdCounter, spell);
        SelfSpellIdCounter++;
        WhoaPlayerProperties.SavePrefs();
        SaveSpells();
    }

    public void SaveSpells()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = File.Open(SELF_SPELLS_PATH, FileMode.OpenOrCreate))
        {
            formatter.Serialize(stream, SelfSpells);
        }
    }
    public void LoadSpells()
    {
        if (!File.Exists(SELF_SPELLS_PATH))
            SaveDefaults();
        FileStream stream = File.Open(SELF_SPELLS_PATH, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SelfSpells = (Dictionary<int, SelfSpell>)formatter.Deserialize(stream);
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            stream.Close();
            SaveDefaults();
        }
    }

    private void SaveDefaults()
    {
        SelfSpells = new Dictionary<int, SelfSpell>();
        SaveSpells();
    }
}

