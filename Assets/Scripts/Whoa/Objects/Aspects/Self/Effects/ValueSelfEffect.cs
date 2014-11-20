using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects.Self.Effects
{
    public abstract class ValueSelfEffect : SelfEffect
    {
        public abstract IEnumerator ApplyEffect(WhoaCharacter character);
    }
}
