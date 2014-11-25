using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[Serializable]
class OLDWhoaCharacterData
{
    public bool Purchased { get; set; }

    public WhoaCharacterStatistics Statistics { get; set; }

    public Dictionary<string, int> UpgradeLevelDatabase { get; set; }
}
