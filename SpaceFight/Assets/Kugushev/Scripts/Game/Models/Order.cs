using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    public class Order : Poolable<Order.State>
    {
        public struct State
        {
            public State(Planet planet)
            {
                SourcePlanet = planet;
                Status = OrderStatus.Created;
            }

            public Planet SourcePlanet;
            public OrderStatus Status;
        }

        // todo: verify assumed capacity
        private readonly List<Vector3> _path = new List<Vector3>(128);

        public Order(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public OrderStatus Status
        {
            get => ObjectState.Status;
            set => ObjectState.Status = value;
        }

        public void RegisterMovement(Vector3 position)
        {
            // todo; register
        }

        protected override void OnClear()
        {
            _path.Clear();
        }

        protected override void OnRestore()
        {
            _path.Clear();
        }
    }
}