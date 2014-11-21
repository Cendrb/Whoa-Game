﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Aspects.Self.Effects
{
    public class HealSelfEffect : StartEndSelfEffect
    {
        public HealSelfEffect(int healAmount)
            : base(0, healAmount)
        {

        }

        public override void Start(PlayerDynamicProperties properties)
        {
            properties.Health += Amplifier;
        }

        public override void End(PlayerDynamicProperties properties)
        {
            
        }
    }
}
