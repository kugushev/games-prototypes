using System;
using Kugushev.Scripts.Common.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.ValueObjects.PlayerProperties
{
    public struct GradatedEffectsBuilder
    {
        public Percentage? UnderCapEffect;
        public float? Cap;
        public Percentage? OverCapEffect;
    }
}