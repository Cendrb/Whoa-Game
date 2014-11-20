using Aspects.Self.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Templates
{
    public class SelfAspectTemplate
    {
        public SelfTemplateType Type { get; private set; }
        public string Description { get; private set; }
        public int DefaultDuration { get; private set; }
        public float DefaultAmplifier { get; private set; }
        public WhoaCharacter RequiredCharacter { get; private set; }
        public int RequiredHighscore { get; private set; }
        public int RequiredMoney { get; private set; }
        public int BasePrice { get; private set; }
        public int BaseKlidCost { get; private set; }
        public float ExpensesMultiplierPerDuration { get; private set; }
        public float ExpensesMultiplierPerAmplifier { get; private set; }

        public Sprite Sprite { get; private set; }

        public SelfAspectTemplate(SelfTemplateType type, string description, int defaultDuration, int defaultAmplifier, WhoaCharacter requiredCharacter, int requiredHighscore, int requiredMoney, int basePrice, int baseKlidCost, float expensesMultiplierPerDuration, float expensesMultiplierPerAmplifier)
        {
            Type = type;
            Description = description;
            DefaultDuration = defaultDuration;
            DefaultAmplifier = defaultAmplifier;
            RequiredCharacter = requiredCharacter;
            RequiredHighscore = requiredHighscore;
            RequiredMoney = requiredMoney;
            BasePrice = basePrice;
            BaseKlidCost = BaseKlidCost;
            ExpensesMultiplierPerDuration = expensesMultiplierPerDuration;
            ExpensesMultiplierPerAmplifier = expensesMultiplierPerAmplifier;

            Sprite = Resources.Load<Sprite>("Graphics/Aspects/Icons/Self/" + Type.ToString());
        }

        public SelfAspect GetAspect()
        {

        }
    }
}
