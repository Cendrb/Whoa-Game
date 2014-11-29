using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SelfTemplateType { BablBam, Parkour, BMW, Regeneration, Heal, Speed, Slowness, Intelligence}

public static class Static
{
    public static string GetName(EffectAffectedProperty affectedProperty)
    {
        switch (affectedProperty)
        {
            case EffectAffectedProperty.health:
                return "Health";
            case EffectAffectedProperty.klid:
                return "Klid";
            case EffectAffectedProperty.klidRegen:
                return "Klid regen";
            default:
                return "Enum not implemented";
        }

    }

    public static void Populate<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }
}

