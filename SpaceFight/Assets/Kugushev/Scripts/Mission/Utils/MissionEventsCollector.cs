using System.Collections.Generic;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Utils
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(MissionEventsCollector))]
    public class MissionEventsCollector : ScriptableObject
    {
        // todo: use array and abuse managed pointers
        private List<MissionEvent> _events = new List<MissionEvent>(512);

        public void Cleanup() => _events.Clear();

        public void PlanetCaptured(Faction newOwner, Faction previousOwner, int overpower) =>
            _events.Add(new MissionEvent(MissionEventType.PlanetCaptured, newOwner, previousOwner, overpower));
    }
}