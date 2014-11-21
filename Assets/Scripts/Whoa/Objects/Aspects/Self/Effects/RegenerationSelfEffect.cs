using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects.Self.Effects
{
    public class RegenerationSelfEffect : LoopSelfEffect
    {
        public RegenerationSelfEffect(int duration, int HPPerSecond)
            : base(duration, 0)
        {

        }

        public override void Cycle(PlayerDynamicProperties properties, int second)
        {
            properties.Health += Amplifier;
        }
    }
}
