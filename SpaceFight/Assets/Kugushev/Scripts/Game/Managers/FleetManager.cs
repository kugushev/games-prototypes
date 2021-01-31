using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Models.Abstractions;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "FleetManager")]
    public class FleetManager : Model
    {
        [SerializeField] private ObjectsPool pool;
        [SerializeField] private float armySpeed = 5f;
        [SerializeField] private float armyAngularSpeed = 1f;

        public Queue<Army> ArmiesToSent { get; } = new Queue<Army>();

        protected override void Dispose(bool destroying)
        {
        }

        public void CommitOrder(Order order)
        {
            order.Commit();
            if (order.SourcePlanet.Power > 0)
            {
                var power = order.SourcePlanet.Recruit();
                var army = pool.GetObject<Army, Army.State>(new Army.State(order, armySpeed, armyAngularSpeed, power));
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