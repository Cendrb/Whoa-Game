using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class WhoaCharacterData
{
    public bool Purchased { get; set; }

    public WhoaCharacterStatistics Statistics { get; set; }

    public Dictionary<string, int> UpgradeLevelDatabase { get; set; }

    public Dictionary<int, int> SelectedSelfSpellsIds { get; set; }

    public Dictionary<int, int> SelectedRangedSpellsIds { get; set; }
}

