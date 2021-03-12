using System.Collections.Generic;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "FleetManager")]
    public class Fleet : ScriptableObject, IModel
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private MissionModelProvider modelProvider;
        [SerializeField] private MissionEventsCollector eventsCollector;
        [SerializeField] private AchievementsManager achievementsManager;
        [SerializeField] private float armySpeed = 0.2f;
        [SerializeField] private float armyAngularSpeed = 1f;
        [SerializeField] private Faction faction;
        private FleetProperties _fleetProperties;

        public Queue<Army> ArmiesToSent { get; } = new Queue<Army>();

        public void SetFleetProperties(FleetProperties fleetProperties) => _fleetProperties = fleetProperties;
        public void ClearFleetProperties() => _fleetProperties = default;

        public void CommitOrder(Order order, Planet target)
        {
            order.Commit(target);
            if (order.SourcePlanet.Power > 0)
            {
                var power = order.SourcePlanet.Recruit(order.Power);
                var army = pool.GetObject<Army, Army.State>(
                    new Army.State(order, armySpeed, armyAngularSpeed, faction, power, in _fleetProperties,
                        eventsCollector));
                ArmiesToSent.Enqueue(army);
                eventsCollector.ArmySent.Add(new ArmySent(faction, power, order.SourcePlanet.Power));
            }
            else
            {
                Debug.LogWarning("Planet power is zero");
                order.Dispose();
            }
        }

        public void Dispose()
        {
            foreach (var army in ArmiesToSent)
                army.Dispose();
        }
    }
}