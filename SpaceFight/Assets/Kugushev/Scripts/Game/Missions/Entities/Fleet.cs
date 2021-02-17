using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Presets;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Entities
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "FleetManager")]
    public class Fleet : ScriptableObject, IModel
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private float armySpeed = 0.2f;
        [SerializeField] private float armyAngularSpeed = 1f;
        [SerializeField] private Faction faction;

        public Queue<Army> ArmiesToSent { get; } = new Queue<Army>();
       
        
        public void Dispose()
        {
            // todo: dispse armies to sent
            // todo: dispose all armies 
        }

        public void CommitOrder(Order order, Planet target)
        {
            order.Commit(target);
            if (order.SourcePlanet.Power > 0)
            {
                var power = order.SourcePlanet.Recruit();
                var army = pool.GetObject<Army, Army.State>(
                    new Army.State(order, armySpeed, armyAngularSpeed, faction, power));
                ArmiesToSent.Enqueue(army);
            }
            else
            {
                // todo: show alert that planet is empty
                order.Dispose();
            }
        }
    }
}