using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public abstract class LoopSelfEffect : SelfEffect
    {
        public LoopSelfEffect(int duration, int amplifier)
            : base(duration, amplifier)
        {

        }

        public override System.Collections.IEnumerator ApplyEffect(PlayerDynamicProperties properties)
        {
            int secondCounter = 0;
            for (int remainingSeconds = Duration; Duration > 0; remainingSeconds--)
            {
                Cycle(properties, secondCounter);
                secondCounter++;
                yield return new WaitForSeconds(1);
            }
        }

        public abstract void Cycle(PlayerDynamicProperties properties, int second);
    }
}
