using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Managers
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