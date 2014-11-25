using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class SpeedSelfEffect : StartEndSelfEffect
    {
        public SpeedSelfEffect(Sprite sprite, int duration, int speedAmount)
            : base(sprite, duration, speedAmount)
        {

        }

        public override void Start(PlayerDynamicProperties properties)
        {
            properties.Speed += Amplifier;
        }

        public override void End(PlayerDynamicProperties properties)
        {
            properties.Speed -= Amplifier;
        }
    }
}
