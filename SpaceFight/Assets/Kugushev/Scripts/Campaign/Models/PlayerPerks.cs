﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    [Serializable]
    public class PlayerPerks : Poolable<PlayerPerks.State>
    {
        public readonly struct State
        {
            public State(IReadOnlyList<PerkId> availablePerks)
            {
                AvailablePerks = availablePerks;
            }

            public IReadOnlyList<PerkId> AvailablePerks { get; }
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
            if (!ObjectState.AvailablePerks.Contains(perkInfo.Id))
            {
                // epic perks are related to selected available list
                return;
            }

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