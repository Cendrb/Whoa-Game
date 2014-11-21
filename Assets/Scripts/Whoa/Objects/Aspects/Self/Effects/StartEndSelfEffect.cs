using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public abstract class StartEndSelfEffect : SelfEffect
    {
        public StartEndSelfEffect(int duration, int amplifier)
            : base(duration, amplifier)
        {

        }

        public override System.Collections.IEnumerator ApplyEffect(PlayerDynamicProperties properties)
        {
            Start(properties);
            yield return new WaitForSeconds(Duration);
            End(properties);
        }

        public abstract void Start(PlayerDynamicProperties properties);
        public abstract void End(PlayerDynamicProperties properties);
       
    }
}
