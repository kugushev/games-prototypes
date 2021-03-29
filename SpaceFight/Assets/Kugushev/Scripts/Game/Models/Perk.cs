using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    public abstract class Perk : ScriptableObject
    {
        public abstract PerkInfo Info { get; }
    }
}