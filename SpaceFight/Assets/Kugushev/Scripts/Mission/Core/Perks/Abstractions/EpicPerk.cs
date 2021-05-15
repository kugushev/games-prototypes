using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Abstractions
{
    public abstract class EpicPerk : BasePerk
    {
        [SerializeField] private int level;

        private PerkInfo? _info;

        public sealed override PerkInfo Info => _info ??= new PerkInfo(
            PerkId, level, PerkType.Epic, Name,
            Criteria,
            Perk);

        protected abstract PerkId PerkId { get; }
        protected abstract string Name { get; }
        protected abstract string Criteria { get; }
        protected abstract string Perk { get; }
    }
}