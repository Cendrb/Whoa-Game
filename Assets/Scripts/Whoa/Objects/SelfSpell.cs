using Aspects.Self;
using Aspects.Self.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class SelfSpell
{
    public List<SelfAspect> Aspects { get; set; }

    [NonSerialized]
    public List<SelfEffect> Effects;

    [NonSerialized]
    int price = -1;

    public SelfSpell()
    {
        Aspects = new List<SelfAspect>();
        Effects = new List<SelfEffect>();

        foreach (SelfAspect aspect in Aspects)
            Effects.Add(aspect.GetEffect());
    }

    public int GetKlidCost(bool calculate)
    {
        if (calculate || price == -1)
        {
            price = 0;
            foreach (SelfAspect aspect in Aspects)
                price += aspect.GetKlidCost();
        }
        return price;
    }
}

