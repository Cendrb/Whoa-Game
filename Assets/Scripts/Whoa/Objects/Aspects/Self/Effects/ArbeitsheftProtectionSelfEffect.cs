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
            properties.SetCollisionHandling(CollisionType.njarbeitsheft1, -1);
            properties.SetCollisionHandling(CollisionType.njarbeitsheft2, -1);
            properties.SetCollisionHandling(CollisionType.njarbeitsheft3, -1);
        }

        public override void End(PlayerDynamicProperties properties)
        {
            properties.RevertCollisionHandling(CollisionType.njarbeitsheft1);
            properties.RevertCollisionHandling(CollisionType.njarbeitsheft2);
            properties.RevertCollisionHandling(CollisionType.njarbeitsheft3);
        }
    }
}
