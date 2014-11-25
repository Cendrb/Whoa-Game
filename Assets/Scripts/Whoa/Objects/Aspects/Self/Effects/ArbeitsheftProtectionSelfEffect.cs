using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class ArbeitsheftProtectionSelfEffect : StartEndSelfEffect
    {
        public ArbeitsheftProtectionSelfEffect(Sprite sprite, int duration)
            : base(sprite, duration, 0)
        {

        }

        public override void Start(PlayerDynamicProperties properties)
        {
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft1, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft2, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft3, -1);
        }

        public override void End(PlayerDynamicProperties properties)
        {
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft1);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft2);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft3);
        }
    }
}
