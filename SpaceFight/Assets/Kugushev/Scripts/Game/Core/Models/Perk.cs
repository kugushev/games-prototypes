using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Models
{
    public abstract class Perk : ScriptableObject
    {
        public abstract PerkInfo Info { get; }
    }
}