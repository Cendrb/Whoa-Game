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
                int minDuration = int.Parse(row.Elements[6].Value);
                int maxDuration = int.Parse(row.Elements[7].Value);
                int minAmplifier = int.Parse(row.Elements[8].Value);
                int maxAmplifier = int.Parse(row.Elements[9].Value);
                string requiredCharacterString = row.Elements[10].Value;
                WhoaCharacter requiredCharacter = null;
                if (requiredCharacterString != null && requiredCharacterString != String.Empty)
                    requiredCharacter = WhoaPlayerProperties.Characters.FindByName(requiredCharacterString);
                int requiredHighscore = int.Parse(row.Elements[11].Value);
                int requiredMoney = int.Parse(row.Elements[12].Value);
                int basePrice = int.Parse(row.Elements[13].Value);
                float baseKlidCost = float.Parse(row.Elements[14].Value);
                float ADPerDuration = float.Parse(row.Elements[15].Value);
                float ADPerAmplifier = float.Parse(row.Elements[16].Value);
                float KlidPerDuration = float.Parse(row.Elements[17].Value);
                float KlidPerAmplifier = float.Parse(row.Elements[18].Value);

                SelfAspect aspectData = new SelfAspect();
                aspectData.Amplifier = amplifier;
                aspectData.AmplifierName = amplifierName;
                aspectData.BaseKlidCost = baseKlidCost;
                aspectData.BasePrice = basePrice;
                aspectData.Description = description;
                aspectData.Duration = duration;
                aspectData.ADPerAmplifier = ADPerAmplifier;
                aspectData.ADPerDuration = ADPerDuration;
                aspectData.KlidPerAmplifier = KlidPerAmplifier;
                aspectData.KlidPerDuration = KlidPerDuration;
                aspectData.Name = name;
                aspectData.Type = type;
                SelfAspectsTemplates.Add(new SelfAspectTemplate(aspectData, requiredCharacter, requiredHighscore, requiredMoney, minDuration, maxDuration, minAmplifier, maxAmplifier));
            }
        }

        public void Load()
        {
            foreach (SelfAspectTemplate template in SelfAspectsTemplates)
                template.Load();
        }
        public void Save()
        {
            foreach (SelfAspectTemplate template in SelfAspectsTemplates)
                template.Save();
        }



    }
}
