using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self
{
    public class SelfAspectTemplate
    {
        public string Name
        {
            get
            {
                return AspectData.Name;
            }
        }

        SelfAspect AspectData { get; set; }
        public WhoaCharacter RequiredCharacter { get; set; }
        public int RequiredHighscore { get; set; }
        public int RequiredMoney { get; set; }

        public Sprite Sprite { get; private set; }

        public SelfAspectTemplate(SelfAspect aspectData, WhoaCharacter requiredCharacter, int requiredHighscore, int requiredMoney)
        {
            AspectData = aspectData;
            RequiredCharacter = requiredCharacter;
            RequiredHighscore = requiredHighscore;
            RequiredMoney = requiredMoney;

            Sprite = aspectData.icon;
        }

        public SelfAspect GetAspect()
        {
            return new SelfAspect()
            {
                Amplifier = AspectData.Amplifier,
                AmplifierName = AspectData.AmplifierName,
                BaseKlidCost = AspectData.BaseKlidCost,
                BasePrice = AspectData.BasePrice,
                Description = AspectData.Description,
                Duration = AspectData.Duration,
                ExpensesMultiplierPerAmplifier = AspectData.ExpensesMultiplierPerAmplifier,
                ExpensesMultiplierPerDuration = AspectData.ExpensesMultiplierPerDuration,
                Name = AspectData.Name,
                Type = AspectData.Type,
            };
        }
    }
}
