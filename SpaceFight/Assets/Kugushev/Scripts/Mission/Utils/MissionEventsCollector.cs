using System;
using System.Collections.Generic;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Utils
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(MissionEventsCollector))]
    public class MissionEventsCollector : ScriptableObject
    {
        [NonSerialized] private readonly List<MissionEvent> _events = new List<MissionEvent>(512);

        public IReadOnlyList<MissionEvent> Events => _events;

        public void Cleanup() => _events.Clear();

        public void PlanetCaptured(Faction newOwner, Faction previousOwner, float overpower) =>
            _events.Add(new MissionEvent(MissionEventType.PlanetCaptured, newOwner, previousOwner, overpower));

        public void ArmyDestroyedInFight(Faction destroyer, Faction victim, float overpower) =>
            _events.Add(new MissionEvent(MissionEventType.ArmyDestroyedInFight, destroyer, victim, overpower));
    }
}