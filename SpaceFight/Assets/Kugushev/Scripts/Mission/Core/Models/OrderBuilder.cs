using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public class OrderBuilder
    {
        private State? _state;

        private struct State
        {
            public State(Planet sourcePlanet, Percentage power)
            {
                Power = power;
                SourcePlanet = sourcePlanet;
                LastRegisteredPosition = null;
                Path = ListPool<Vector3>.Instance.Spawn();
            }

            public readonly Planet SourcePlanet;
            public readonly Percentage Power;
            public Vector3? LastRegisteredPosition;
            public readonly List<Vector3> Path;
        }

        public Planet SourcePlanet => _state?.SourcePlanet ?? throw new SpaceFightException("Invalid order builder");
        public Percentage Power => _state?.Power ?? throw new SpaceFightException("Invalid order builder");

        public void StartCreating(Planet sourcePlanet, Percentage power)
        {
            Rollback();
            _state = new State(sourcePlanet, power);
        }

        internal void RegisterMovement(Vector3 position,
            float gapBetweenWaypoints = GameplayConstants.GapBetweenWaypoints)
        {
            var state = _state ?? throw new SpaceFightException("State is null");

            state.LastRegisteredPosition = position;

            if (state.Path.Count > 0)
            {
                var last = state.Path.Last();
                if (Vector3.Distance(position, last) < gapBetweenWaypoints)
                    return;
            }

            if (state.Path.Capacity > GameplayConstants.OrderPathCapacity)
                Debug.LogWarning($"Path capacity increased to {state.Path.Capacity}");

            state.Path.Add(position);
        }

        public Order Commit(Planet target)
        {
            var state = _state ?? throw new SpaceFightException("State is null");

            if (state.LastRegisteredPosition != null)
                state.Path.Add(state.LastRegisteredPosition.Value);

            return new Order(state.SourcePlanet, target, state.Power, state.Path);
        }

        public void Rollback()
        {
            if (_state == null)
                return;

            ListPool<Vector3>.Instance.Despawn(_state.Value.Path);
            _state = default;
        }
    }
}