using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Whoa.Objects;

[Serializable]
public class WhoaCharacterData
{
    public WhoaCharacterUpgrades Upgrades { get; set; }
    public WhoaCharacterStatistics Statistics { get; set; }
}

