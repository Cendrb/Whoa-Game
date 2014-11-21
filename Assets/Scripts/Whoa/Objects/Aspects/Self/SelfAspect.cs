using Aspects.Self.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self
{
    [Serializable]
    public class SelfAspect
    {
        public SelfTemplateType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string AmplifierName { get; set; }
        public int Amplifier { get; set; }
        public int BasePrice { get; set; }
        public int BaseKlidCost { get; set; }
        public float ExpensesMultiplierPerDuration { get; set; }
        public float ExpensesMultiplierPerAmplifier { get; set; }

        public int GetPrice()
        {
            return (int)(BasePrice * Mathf.Pow(ExpensesMultiplierPerAmplifier, Amplifier) * Mathf.Pow(ExpensesMultiplierPerDuration, Duration));
        }
        public int GetKlidCost()
        {
            return (int)(BaseKlidCost * Mathf.Pow(ExpensesMultiplierPerAmplifier, Amplifier) * Mathf.Pow(ExpensesMultiplierPerDuration, Duration));
        }
        public SelfEffect GetEffect()
        {
            switch(Type)
            {
                case SelfTemplateType.BablBam:
                    return new BablBamEffect(Duration);
                case SelfTemplateType.Speed:
                    return new SpeedSelfEffect(Duration, Amplifier);
                case SelfTemplateType.Slowness:
                    return new SlownessSelfEffect(Duration, Amplifier);
                case SelfTemplateType.Inteligence:
                    return new ArbeitsheftProtectionSelfEffect(Duration);
                case SelfTemplateType.Parkour:
                    return new ParkourSelfEffect(Duration);
                case SelfTemplateType.Heal:
                    return new HealSelfEffect(Amplifier);
                case SelfTemplateType.Regeneration:
                    return new RegenerationSelfEffect(Duration, Amplifier);
            }
            return null;
        }
    }
}
