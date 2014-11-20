using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects.Self.Aspects
{
    public abstract class SelfAspect
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Duration { get; private set; }
        public int Amplifier { get; private set; }

        public SelfAspect(string name, string description, int duration)
        {
            Name = name;
            Description = description;
            Duration = duration;
        }

        public abstract int GetPrice();
    }
}
