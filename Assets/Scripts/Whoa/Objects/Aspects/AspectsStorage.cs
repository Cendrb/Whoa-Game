using Aspects.Self;
using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects
{
    public class AspectsTemplatesStorage
    {
        public List<SelfAspectTemplate> SelfAspectsTemplates { get; private set; }

        public AspectsTemplatesStorage()
        {
            SelfAspectsTemplates = new List<SelfAspectTemplate>();
            ListFeed list = GDriveManager.GetSpreadsheet(WhoaPlayerProperties.DRIVE_DOCUMENT_URL, 3);
            foreach (ListEntry row in list.Entries)
            {
                SelfTemplateType type = (SelfTemplateType)Enum.Parse(typeof(SelfTemplateType), row.Elements[0].Value);
                string name = row.Elements[1].Value;
                string amplifierName = row.Elements[2].Value;
                string description = row.Elements[3].Value;
                int duration = int.Parse(row.Elements[4].Value);
                int amplifier = int.Parse(row.Elements[5].Value);
                string requiredCharacterString = row.Elements[6].Value;
                WhoaCharacter requiredCharacter = null;
                if (requiredCharacterString != null && requiredCharacterString != String.Empty)
                    requiredCharacter = WhoaPlayerProperties.Characters.FindByName(requiredCharacterString);
                int requiredHighscore = int.Parse(row.Elements[7].Value);
                int requiredMoney = int.Parse(row.Elements[8].Value);
                int basePrice = int.Parse(row.Elements[9].Value);
                float baseKlidCost = float.Parse(row.Elements[10].Value);
                float expensesMultiplierPerDuration = float.Parse(row.Elements[11].Value);
                float expensesMultiplierPerAmplifier = float.Parse(row.Elements[12].Value);
                SelfAspect aspectData = new SelfAspect();
                aspectData.Amplifier = amplifier;
                aspectData.AmplifierName = amplifierName;
                aspectData.BaseKlidCost = baseKlidCost;
                aspectData.BasePrice = basePrice;
                aspectData.Description =  description;
                aspectData.Duration = duration;
                aspectData.ExpensesMultiplierPerAmplifier = expensesMultiplierPerAmplifier;
                aspectData.ExpensesMultiplierPerDuration = expensesMultiplierPerDuration;
                aspectData.Name = name;
                aspectData.Type = type;
                SelfAspectsTemplates.Add(new SelfAspectTemplate(aspectData, requiredCharacter, requiredHighscore, requiredMoney));
            }
        }


    }
}
