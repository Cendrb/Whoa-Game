using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspects.Self.Effects
{
    public class ParkourSelfEffect : StartEndSelfEffect
    {
        public ParkourSelfEffect(int duration)
            : base(duration, 0)
        {

        }

        public override void Start(PlayerDynamicProperties properties)
        {
            properties.SetCollisionHandling(KillerCollisionScript.CollisionType.wall, 0);
        }

        public override void End(PlayerDynamicProperties properties)
        {
            properties.RevertCollisionHandling(KillerCollisionScript.CollisionType.wall);
        }
    }
}
