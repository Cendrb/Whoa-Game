using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum EffectAffectedProperty { health, klid, klidRegen, speed, flap, gravity }
public enum EffectMethod { plus, times }

public class UpgradeEffect
{
    public EffectAffectedProperty AffectedProperty { get; private set; }

    private float[] propertyModifiers;
    private EffectMethod method;

    public UpgradeEffect(EffectAffectedProperty property, EffectMethod method, float[] modifiers)
    {
        AffectedProperty = property;
        propertyModifiers = modifiers;
        this.method = method;
    }

    public float GetModifiedValue(float source, int level)
    {
        float modifier = 1;
        if (method == EffectMethod.times)
        {
            for (int x = level; x != 0; x--)
                modifier *= propertyModifiers[x - 1];
            return source * modifier;
        }
        else
        {
            for (int x = level; x != 0; x--)
                source += propertyModifiers[x - 1];
            return source;
        }

    }
}

