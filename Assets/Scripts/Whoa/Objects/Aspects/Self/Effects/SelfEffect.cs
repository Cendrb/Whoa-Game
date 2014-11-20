using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects.Self.Effects
{
    public abstract class SelfEffect
    {
        public int Duration { get; set; }

        public SelfEffect(int duration)
        {
            Duration = duration;
        }
    }
}
