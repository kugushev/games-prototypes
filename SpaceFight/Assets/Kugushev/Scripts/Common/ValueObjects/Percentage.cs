using UnityEngine;

namespace Kugushev.Scripts.Common.ValueObjects
{
    public readonly struct Percentage
    {
        public Percentage(float amount) => Amount = amount;
        public Percentage(int percent) => Amount = percent / 100f;

        public float Amount { get; }
    }
}