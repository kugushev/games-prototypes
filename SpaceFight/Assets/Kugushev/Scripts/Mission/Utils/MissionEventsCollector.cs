using System.Collections.Generic;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Utils
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(MissionEventsCollector))]
    public class MissionEventsCollector : ScriptableObject
    {
        private const int Capacity = 128;

        public List<PlanetCaptured> PlanetCaptured { get; } = new List<PlanetCaptured>(Capacity);
        public List<ArmyDestroyedInFight> ArmyDestroyedInFight { get; } = new List<ArmyDestroyedInFight>(Capacity);
        public List<ArmySent> ArmySent { get; } = new List<ArmySent>(Capacity);

        public void Cleanup()
        {
            PlanetCaptured.Clear();
            ArmyDestroyedInFight.Clear();
        }
    }
}