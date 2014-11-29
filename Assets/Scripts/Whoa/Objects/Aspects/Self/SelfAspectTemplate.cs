using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        string savePath;

        public SelfAspectTemplateData Data { get; private set; }
        public WhoaCharacter RequiredCharacter { get; private set; }
        public int RequiredHighscore { get; private set; }
        public int RequiredMoney { get; private set; }

        public int MinDuration { get; private set; }
        public int MaxDuration { get; private set; }
        public int MinAmplifier { get; private set; }
        public int MaxAmplifier { get; private set; }
        public int DefaultAmplifier
        {
            get
            {
                return AspectData.Amplifier;
            }
        }
        public int DefaultDuration
        {
            get
            {
                return AspectData.Duration;
            }
        }

        public string AmplifierName
        {
            get
            {
                return AspectData.AmplifierName;
            }
        }

        public Sprite Sprite { get; private set; }

        public SelfAspectTemplate(SelfAspect aspectData, WhoaCharacter requiredCharacter, int requiredHighscore, int requiredMoney, int minDuration, int maxDuration, int minAmplifier, int maxAmplifier)
        {
            AspectData = aspectData;
            RequiredCharacter = requiredCharacter;
            RequiredHighscore = requiredHighscore;
            RequiredMoney = requiredMoney;

            MinDuration = minDuration;
            MaxDuration = maxDuration;
            MinAmplifier = minAmplifier;
            MaxAmplifier = maxAmplifier;

            Sprite = aspectData.icon;

            savePath = Application.persistentDataPath + "/" + AspectData.Type.ToString() + ".dat";

            Load();
        }

        public void Load()
        {
            if (!File.Exists(savePath))
            {
                SaveDefaults();
            }
            else
            {
                FileStream stream = File.Open(savePath, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Data = (SelfAspectTemplateData)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch (Exception e)
                {
                    stream.Close();
                    Debug.LogException(e);
                    SaveDefaults();
                }
            }
        }

        private void SaveDefaults()
        {
            Data = new SelfAspectTemplateData();
            Data.Bought = false;
            Save();
        }

        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Open(savePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(stream, Data);
            }
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

        [Serializable]
        public class SelfAspectTemplateData
        {
            public bool Bought { get; set; }
        }
    }
}
