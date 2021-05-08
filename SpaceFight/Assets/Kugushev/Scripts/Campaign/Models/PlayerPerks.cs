using System;
using System.Collections.Generic;
using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    [Serializable]
    public class PlayerPerks : PoolableOld<PlayerPerks.State>
    {
        public readonly struct State
        {
            public State(ISet<PerkId> availablePerks)
            {
                AvailablePerks = availablePerks;
            }

            public ISet<PerkId> AvailablePerks { get; }
        }

        public PlayerPerks(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        [SerializeReference] private List<PerkInfo> commonPerks = new List<PerkInfo>();

        // stop thinking that using struct in dictionary cause boxing. It's not true right now, just turn on profiler and check
        [SerializeReference] private Dictionary<PerkId, PerkInfo> epicPerks =
            new Dictionary<PerkId, PerkInfo>();

        public IReadOnlyList<PerkInfo> CommonPerks => commonPerks;
        public IReadOnlyDictionary<PerkId, PerkInfo> EpicPerks => epicPerks;

        public ISet<PerkId> AvailablePerks => ObjectState.AvailablePerks;

        internal void AddPerk(PerkInfo perkInfo)
        {
            switch (perkInfo.Type)
            {
                case PerkType.Common:
                    AddCommon(perkInfo);
                    break;
                case PerkType.Epic:
                    AddEpic(perkInfo);
                    break;
                default:
                    Debug.LogError($"Unexpected achievement type {perkInfo}");
                    break;
            }
        }

        private void AddCommon(PerkInfo perkInfo)
        {
            if (perkInfo.Level != null)
                Debug.LogError($"Common achievement with non null level: {perkInfo}");

            commonPerks.Add(perkInfo);
        }

        private void AddEpic(PerkInfo perkInfo)
        {
            if (epicPerks.TryGetValue(perkInfo.Id, out var current) &&
                (current.Level >= perkInfo.Level || perkInfo.Level == null || current.Level == null))
            {
                Debug.LogError($"Unexpected achievement. Current {current}, new {perkInfo}");
                return;
            }

            epicPerks[perkInfo.Id] = perkInfo;
        }

        protected override void OnClear(State state)
        {
            commonPerks.Clear();
            epicPerks.Clear();
        }
    }
}