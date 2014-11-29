using Aspects.Self;
using Aspects.Self.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SelfSpell
{
    public string Name { get; set; }
    public string Abbreviate { get; set; }

    public List<SelfAspect> Aspects { get; set; }

    [NonSerialized]
    public List<SelfEffect> Effects;

    public SelfSpell()
    {
        Aspects = new List<SelfAspect>();
        Effects = new List<SelfEffect>();
    }

    public int GetKlidCost()
    {
        int klidPrice = 0;
        foreach (SelfAspect aspect in Aspects)
            klidPrice += aspect.GetKlidCost();
        klidPrice = (int)(klidPrice * Mathf.Pow(1.05f, Aspects.Count));
        return klidPrice;
    }

    public int GetADCost()
    {
        int adPrice = 0;
        foreach (SelfAspect aspect in Aspects)
            adPrice += aspect.GetPrice();
        adPrice = (int)(adPrice * Mathf.Pow(1.05f, Aspects.Count));
        return adPrice;
    }

    public void GenerateEffects()
    {
        if (Effects == null)
            Effects = new List<SelfEffect>();
        foreach (SelfAspect aspect in Aspects)
            Effects.Add(aspect.GetEffect());
    }
}

