using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class RegenerationSelfEffect : LoopSelfEffect
    {
        public RegenerationSelfEffect(Sprite sprite, int duration, int HPPerSecond)
            : base(sprite, duration, HPPerSecond)
        {

        }

        public override void Cycle(PlayerDynamicProperties properties, int second)
        {
            properties.Health += Amplifier;
        }
    }
}
