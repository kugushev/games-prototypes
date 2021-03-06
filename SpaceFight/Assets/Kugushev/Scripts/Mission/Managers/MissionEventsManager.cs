using System.Collections.Generic;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Mission Events Manager")]
    public class MissionEventsManager : ScriptableObject
    {
        // todo: use array and abuse managed pointers
        private List<MissionEvent> _events = new List<MissionEvent>(512);

        public void PlanetCaptured(Faction newOwner, Faction previousOwner, int overpower) =>
            _events.Add(new MissionEvent(MissionEventType.PlanetCaptured, newOwner, previousOwner, overpower));
    }
}