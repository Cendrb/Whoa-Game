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
        List<CharacterUpgrade> upgrades = new List<CharacterUpgrade>();

        CharacterUpgrade healthUpgrade = new CharacterUpgrade("Health", 10, 100, (float)3);
        float[] modifiersHealth = new float[healthUpgrade.MaxLevel];
        for(int x = 0; x < modifiersHealth.Length; x++)
            modifiersHealth[x] = (float)1.5;
        healthUpgrade.Effects.Add(new UpgradeEffect(EffectAffectedProperty.health, EffectMethod.times, modifiersHealth));
        upgrades.Add(healthUpgrade);

        CharacterUpgrade klidUpgrade = new CharacterUpgrade("Klid", 10, 200, (float)2);
        float[] modifiersKlid = new float[healthUpgrade.MaxLevel];
        for(int x = 0; x < modifiersKlid.Length; x++)
            modifiersKlid[x] = (float)1.5;
        klidUpgrade.Effects.Add(new UpgradeEffect(EffectAffectedProperty.klid, EffectMethod.times, modifiersKlid));
        klidUpgrade.Effects.Add(new UpgradeEffect(EffectAffectedProperty.klidRegen, EffectMethod.times, modifiersKlid));
        upgrades.Add(klidUpgrade);

        WhoaCharacter andrs = new WhoaCharacter("Andršová", 1.12F, 5, 23F, 377F, 6F, 200, 0.01F, 1, 69, upgrades);
        andrs.Data.Purchased = true;
        andrs.Save();
        characters.Add(andrs);

        upgrades = new List<CharacterUpgrade>();

        healthUpgrade = new CharacterUpgrade("Health", 10, 100, (float)3);
        modifiersHealth = new float[healthUpgrade.MaxLevel];
        for (int x = 0; x < modifiersHealth.Length; x++)
            modifiersHealth[x] = (float)1.5;
        healthUpgrade.Effects.Add(new UpgradeEffect(EffectAffectedProperty.health, EffectMethod.times, modifiersHealth));
        upgrades.Add(healthUpgrade);

        klidUpgrade = new CharacterUpgrade("Klid", 10, 200, (float)2);
        modifiersKlid = new float[healthUpgrade.MaxLevel];
        for (int x = 0; x < modifiersKlid.Length; x++)
            modifiersKlid[x] = (float)1.5;
        klidUpgrade.Effects.Add(new UpgradeEffect(EffectAffectedProperty.klid, EffectMethod.times, modifiersKlid));
        klidUpgrade.Effects.Add(new UpgradeEffect(EffectAffectedProperty.klidRegen, EffectMethod.times, modifiersKlid));
        upgrades.Add(klidUpgrade);

        characters.Add(new WhoaCharacter("Roko", 1.16F, 20, 27F, 377F, 7F, 210, 0.05F, 1, 1000, upgrades));
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

