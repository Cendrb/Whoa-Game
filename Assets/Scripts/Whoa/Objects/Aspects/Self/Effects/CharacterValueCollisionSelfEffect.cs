using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects.Self.Effects
{
    public abstract class SelfEffect
    {
        public int Duration { get; set; }
        public int Amplifier { get; set; }

        public SelfEffect(int duration, int amplifier)
        {
            Amplifier = amplifier;
            Duration = duration;
        }
        public abstract IEnumerator ApplyEffect(PlayerDynamicProperties properties);
    }
}
