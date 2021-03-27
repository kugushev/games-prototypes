using System;
using System.Collections.Generic;
using System.Diagnostics;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Utils
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(MissionEventsCollector))]
    public class MissionEventsCollector : ScriptableObject
    {
        private const int Capacity = 128;
        private Stopwatch _stopwatch;

        public List<PlanetCaptured> PlanetCaptured { get; } = new List<PlanetCaptured>(Capacity);
        public List<ArmyDestroyedInFight> ArmyDestroyedInFight { get; } = new List<ArmyDestroyedInFight>(Capacity);
        public List<ArmyDestroyedOnSiege> ArmyDestroyedOnSiege { get; } = new List<ArmyDestroyedOnSiege>(Capacity);
        public List<ArmySent> ArmySent { get; } = new List<ArmySent>(Capacity);
        public List<ArmyArrived> ArmyArrived { get; } = new List<ArmyArrived>(Capacity);

        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public void Start()
        {
            _stopwatch ??= new Stopwatch();
            _stopwatch.Start();
        }

        public void Stop() => _stopwatch.Stop();

        public void Cleanup()
        {
            PlanetCaptured.Clear();
            ArmyDestroyedInFight.Clear();
            ArmyDestroyedOnSiege.Clear();
            ArmySent.Clear();
            ArmyArrived.Clear();
        }
    }
}