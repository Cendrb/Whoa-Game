using Google.GData.Spreadsheets;
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

    public WhoaCharacters()
    {
        SetupCharacters();
    }

    public void SetupCharacters()
    {
        characters = new List<WhoaCharacter>();

        ListFeed list = GDriveManager.GetSpreadsheet(WhoaPlayerProperties.DRIVE_DOCUMENT_URL, 1);
        foreach (ListEntry row in list.Entries)
        {
            string name = row.Elements[0].Value;
            float multiplier = float.Parse(row.Elements[1].Value);
            int health = int.Parse(row.Elements[2].Value);
            int klid = int.Parse(row.Elements[3].Value);
            float klidRegen = float.Parse(row.Elements[4].Value);
            float speed = float.Parse(row.Elements[5].Value);
            float whoaPower = float.Parse(row.Elements[6].Value);
            float weight = float.Parse(row.Elements[7].Value);
            int spellSlotCount = int.Parse(row.Elements[8].Value);
            int price = int.Parse(row.Elements[9].Value);
            WhoaCharacter character = new WhoaCharacter(name, multiplier, health, whoaPower, speed, weight, klid, klidRegen, spellSlotCount, price);
            characters.Add(character);
        }

        ListFeed upgradesList = GDriveManager.GetSpreadsheet(WhoaPlayerProperties.DRIVE_DOCUMENT_URL, 2);
        WhoaCharacter currentlyUpgradedCharacter;
        CharacterUpgrade upgrade = new CharacterUpgrade("You shall not pass!", 69, 69, 69);
        foreach (ListEntry row in upgradesList.Entries)
        {
            int id = int.Parse(row.Elements[0].Value);
            if (id == -1)
            {
                upgrade.Effects.Add(parseEffect(row));
            }
            else
            {
                currentlyUpgradedCharacter = characters[id];
                
                string name = row.Elements[1].Value;
                int maxLevel = int.Parse(row.Elements[2].Value);
                int basePrice = int.Parse(row.Elements[3].Value);
                float priceMultiplier = float.Parse(row.Elements[4].Value);
                upgrade = new CharacterUpgrade(name, maxLevel, basePrice, priceMultiplier);

                upgrade.Effects.Add(parseEffect(row));

                currentlyUpgradedCharacter.AddUpgrade(upgrade);
            }
        }

        foreach (WhoaCharacter character in characters)
            character.LoadEverything();

    }
    private UpgradeEffect parseEffect(ListEntry row)
    {
        int indexOffset = 5;

        EffectAffectedProperty affectedProperty = (EffectAffectedProperty)Enum.Parse(typeof(EffectAffectedProperty), row.Elements[indexOffset].Value);
        EffectMethod effectMethod = (EffectMethod)Enum.Parse(typeof(EffectMethod), row.Elements[indexOffset + 1].Value);

        indexOffset += 2;

        float[] modifiers = new float[10];
        for (int x = 0; x < 10; x++)
            modifiers[x] = float.Parse(row.Elements[x + indexOffset].Value);

        return new UpgradeEffect(affectedProperty, effectMethod, modifiers);
    }

    public WhoaCharacter FindByName(string name)
    {
        foreach (WhoaCharacter character in characters)
            if (character.Name == name)
                return character;
        return null;
    }

    public void WipeCharactersData()
    {
        Debug.Log("Wiping characters data...");
        foreach (WhoaCharacter character in characters)
            character.SaveDefaults();
        WhoaPlayerProperties.SetCharacter(0);
        characters[0].Data.Purchased = true;
    }

    public void Load()
    {
        Debug.Log("Loading characters data...");
        foreach (WhoaCharacter character in characters)
            character.Load();
    }

    public void Save()
    {
        Debug.Log("Saving characters data...");
        foreach (WhoaCharacter character in characters)
            character.Save();
    }

}

