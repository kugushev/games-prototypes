using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Game.Missions.Presets;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Entities
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "FleetManager")]
    public class Fleet : ScriptableObject, IModel
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private MissionManager missionManager;
        [SerializeField] private float armySpeed = 0.2f;
        [SerializeField] private float armyAngularSpeed = 1f;
        [SerializeField] private Faction faction;

        public Queue<Army> ArmiesToSent { get; } = new Queue<Army>();


        public void CommitOrder(Order order, Planet target)
        {
            order.Commit(target);
            if (order.SourcePlanet.Power > 0)
            {
                var power = order.SourcePlanet.Recruit(order.Power);
                var army = pool.GetObject<Army, Army.State>(
                    new Army.State(order, armySpeed, armyAngularSpeed, faction, power, missionManager.EventsManager));
                ArmiesToSent.Enqueue(army);
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