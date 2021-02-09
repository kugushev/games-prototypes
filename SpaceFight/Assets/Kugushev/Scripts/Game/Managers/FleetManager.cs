using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Entities.Abstractions;
using Kugushev.Scripts.Game.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "FleetManager")]
    public class FleetManager : Model
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private float armySpeed = 5f;
        [SerializeField] private float armyAngularSpeed = 1f;
        [SerializeField] private Faction faction;

        public Queue<Army> ArmiesToSent { get; } = new Queue<Army>();

        protected override void Dispose(bool destroying)
        {
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