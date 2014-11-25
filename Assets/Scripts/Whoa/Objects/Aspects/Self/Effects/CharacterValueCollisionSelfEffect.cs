using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public abstract class SelfEffect
    {
        public int Duration { get; set; }
        public int Amplifier { get; set; }
        public Sprite Sprite { get; set; }

        public SelfEffect(Sprite sprite, int duration, int amplifier)
        {
            Amplifier = amplifier;
            Duration = duration;
            Sprite = sprite;
        }
        public abstract IEnumerator ApplyEffect(PlayerDynamicProperties properties);
    }
}
