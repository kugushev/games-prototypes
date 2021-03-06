using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Entities
{
    [Serializable]
    public class Order : Poolable<Order.State>
    {
        [Serializable]
        public struct State
        {
            public State(Planet planet, Percentage power)
            {
                Power = power;
                sourcePlanet = planet;
                targetPlanet = null;
                status = OrderStatus.Created;
                LastRegisteredPosition = null;
            }

            public Planet sourcePlanet;
            public Percentage Power;
            [CanBeNull] public Planet targetPlanet;
            public OrderStatus status;
            public Vector3? LastRegisteredPosition;
        }

        [SerializeField] private List<Vector3> path = new List<Vector3>(GameConstants.OrderPathCapacity);

        public Order(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public IReadOnlyList<Vector3> Path => path;
        public Planet SourcePlanet => ObjectState.sourcePlanet;
        public Planet TargetPlanet => ObjectState.targetPlanet;
        public Percentage Power => ObjectState.Power;

        public OrderStatus Status
        {
            get => ObjectState.status;
            internal set => ObjectState.status = value;
        }

        internal void RegisterMovement(Vector3 position, float gapBetweenWaypoints = GameConstants.GapBetweenWaypoints)
        {
            ObjectState.LastRegisteredPosition = position;

            if (path.Count > 0)
            {
                var last = path[path.Count - 1];
                if (Vector3.Distance(position, last) < gapBetweenWaypoints)
                    return;
            }

            if (path.Capacity > GameConstants.OrderPathCapacity)
                Debug.LogWarning($"Path capacity increased to {path.Capacity}");

            path.Add(position);
        }

        public void Commit(Planet target)
        {
            if (ObjectState.LastRegisteredPosition != null)
                path.Add(ObjectState.LastRegisteredPosition.Value);

            ObjectState.targetPlanet = target;
            ObjectState.status = OrderStatus.Execution;
        }

        protected override void OnClear(State state) => path.Clear();

        protected override void OnRestore(State state) => path.Clear();
    }
}