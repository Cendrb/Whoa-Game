using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class HealSelfEffect : ValueSelfEffect
    {
        public HealSelfEffect()

        public override System.Collections.IEnumerator ApplyEffect(WhoaCharacter character)
        {
            character.Health += (int) ChangeValue;
            yield new WaitForSeconds(Duration);
        }
    }
}
