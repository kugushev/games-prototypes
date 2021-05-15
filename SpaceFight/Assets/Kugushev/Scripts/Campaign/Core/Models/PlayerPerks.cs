using System.Collections.Generic;
using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models
{
    public interface IPlayerPerks
    {
        ISet<PerkId> AvailablePerks { get; }
        IReadOnlyList<PerkInfo> CommonPerks { get; }
        IReadOnlyDictionary<PerkId, PerkInfo> EpicPerks { get; }
        internal void ObtainPerk(PerkInfo perkInfo);
    }

    internal class PlayerPerks : IPlayerPerks
    {
        private ISet<PerkId>? _availablePerks;
        private readonly List<PerkInfo> _commonPerks = new List<PerkInfo>();
        private readonly Dictionary<PerkId, PerkInfo> _epicPerks = new Dictionary<PerkId, PerkInfo>();

        internal void Init(ISet<PerkId> availablePerks)
        {
            if (_availablePerks != null)
                throw new SpaceFightException("Player Perks are already initialized");

            _availablePerks = availablePerks;
        }

        public ISet<PerkId> AvailablePerks =>
            _availablePerks ?? throw new SpaceFightException("Player Perks are not initialized");

        public IReadOnlyList<PerkInfo> CommonPerks => _commonPerks;
        public IReadOnlyDictionary<PerkId, PerkInfo> EpicPerks => _epicPerks;

        void IPlayerPerks.ObtainPerk(PerkInfo perkInfo)
        {
            switch (perkInfo.Type)
            {
                case PerkType.Common:
                    ObtainCommon(perkInfo);
                    break;
                case PerkType.Epic:
                    ObtainEpic(perkInfo);
                    break;
                default:
                    Debug.LogError($"Unexpected perk type {perkInfo}");
                    break;
            }
        }

        private void ObtainCommon(PerkInfo perkInfo)
        {
            if (perkInfo.Level != null)
                Debug.LogError($"Common perk with non null level: {perkInfo}");

            _commonPerks.Add(perkInfo);
        }

        private void ObtainEpic(PerkInfo perkInfo)
        {
            if (_epicPerks.TryGetValue(perkInfo.Id, out var current) &&
                (current.Level >= perkInfo.Level || perkInfo.Level == null || current.Level == null))
            {
                Debug.LogError($"Unexpected perk. Current {current}, new {perkInfo}");
                return;
            }

            _epicPerks[perkInfo.Id] = perkInfo;
        }
    }
}