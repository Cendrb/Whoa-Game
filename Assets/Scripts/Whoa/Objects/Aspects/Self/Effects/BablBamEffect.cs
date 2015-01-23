using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class BablBamEffect : StartEndSelfEffect
    {
        public BablBamEffect(Sprite sprite, int duration)
            : base(sprite, duration, 0)
        {

        }

        public override void Start(PlayerDynamicProperties properties)
        {
            properties.SetCollisionHandling(CollisionType.basicObstacle, -1);
            properties.SetCollisionHandling(CollisionType.njarbeitsheft1, -1);
            properties.SetCollisionHandling(CollisionType.njarbeitsheft2, -1);
            properties.SetCollisionHandling(CollisionType.njarbeitsheft3, -1);
            properties.SetCollisionHandling(CollisionType.border, -1);
            properties.SetCollisionHandling(CollisionType.zidan, -1);
        }

        public override void End(PlayerDynamicProperties properties)
        {
            properties.RevertCollisionHandling(CollisionType.basicObstacle);
            properties.RevertCollisionHandling(CollisionType.njarbeitsheft1);
            properties.RevertCollisionHandling(CollisionType.njarbeitsheft2);
            properties.RevertCollisionHandling(CollisionType.njarbeitsheft3);
            properties.RevertCollisionHandling(CollisionType.border);
            properties.RevertCollisionHandling(CollisionType.zidan);
        }
    }
}
