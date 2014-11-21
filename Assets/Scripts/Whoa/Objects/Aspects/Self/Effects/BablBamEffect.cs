using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class BablBamEffect : StartEndSelfEffect
    {
        public BablBamEffect(int duration)
            : base(duration, 0)
        {

        }

        public override void Start(PlayerDynamicProperties properties)
        {
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.basicObstacle, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft1, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft2, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft3, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.wall, -1);
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.zidan, -1);
        }

        public override void End(PlayerDynamicProperties properties)
        {
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.basicObstacle);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft1);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft2);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.njarbeitsheft3);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.wall);
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.zidan);
        }
    }
}
