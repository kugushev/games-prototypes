using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
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

            public readonly Planet SourcePlanet;
            public OrderStatus Status;
        }
        
        private readonly List<Vector3> _path = new List<Vector3>(GameConstants.OrderPathCapacity);

        public Order(ObjectsPool objectsPool) : base(objectsPool)
        {
        }
        
        public IReadOnlyList<Vector3> Path => _path;
        
        public OrderStatus Status
        {
            get => ObjectState.Status;
            internal set => ObjectState.Status = value;
        }

        internal void RegisterMovement(Vector3 position, float gapBetweenWaypoints)
        {
            if (_path.Count > 0)
            {
                var last = _path[_path.Count - 1];
                if (Vector3.Distance(position, last) < gapBetweenWaypoints)
                    return;
            }

            if (_path.Capacity > GameConstants.OrderPathCapacity)
                Debug.LogWarning($"Path capacity increased to {_path.Capacity}");

            _path.Add(position);
        }

        protected override void OnClear(State state)
        {
            _path.Clear();
        }

        protected override void OnRestore(State state)
        {
            _path.Clear();
        }
    }
}