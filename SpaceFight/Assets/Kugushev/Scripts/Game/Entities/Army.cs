using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Game.Entities
{
    public class Army : Poolable<Army.State>, IGameLoopParticipant
    {
        public Army(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public struct State
        {
            public readonly Order Order;
            public readonly float Speed;
            public readonly float AngularSpeed;
            public readonly Faction Faction;
            public int Power;
            public ArmyStatus Status;
            public Vector3 CurrentPosition;
            public Quaternion CurrentRotation;
            public int CurrentWaypoint;
            public float WaypointMovingProgress;
            public float WaypointRotationProgress;
            [CanBeNull] public IFighter Target; // todo: support multiple enemies
            public float FightingTimeCollector;

            public State(Order order, float speed, float angularSpeed, Faction faction, int power)
            {
                // todo: assert order
                Order = order;
                Speed = speed;
                AngularSpeed = angularSpeed;
                Faction = faction;
                Power = power;
                Status = ArmyStatus.Recruiting;
                CurrentPosition = order.Path[0];
                CurrentRotation = Quaternion.identity;
                CurrentWaypoint = 0;
                WaypointMovingProgress = 0f;
                WaypointRotationProgress = 0f;
                Target = null;
                FightingTimeCollector = 0f;
            }
        }

        public ArmyStatus Status
        {
            get => ObjectState.Status;
            set => ObjectState.Status = value;
        }

        public Vector3 Position => ObjectState.CurrentPosition;
        public Quaternion Rotation => ObjectState.CurrentRotation;
        public int Power => ObjectState.Power;
        public bool Disbanded => ObjectState.Status == ArmyStatus.Disbanded;
        public Faction Faction => ObjectState.Faction;

        public void NextStep(float deltaTime)
        {
            if (!Active)
                return;

            switch (ObjectState.Status)
            {
                case ArmyStatus.Unspecified:
                    Debug.LogWarning("Army status is Unspecified");
                    break;
                case ArmyStatus.OnMatch:
                case ArmyStatus.Arriving:
                    MoveStep(deltaTime);
                    break;
                case ArmyStatus.Fighting:
                    FightStep(deltaTime);
                    break;
            }
        }

        private void MoveStep(float deltaTime)
        {
            if (ObjectState.WaypointMovingProgress >= 1)
            {
                ObjectState.CurrentWaypoint++;
                ObjectState.WaypointMovingProgress = 0f;
                ObjectState.WaypointRotationProgress = 0f;
            }

            if (ObjectState.Order.Path.Count <= ObjectState.CurrentWaypoint + 1)
            {
                Arrived();
                return;
            }

            var previous = ObjectState.Order.Path[ObjectState.CurrentWaypoint];
            var next = ObjectState.Order.Path[ObjectState.CurrentWaypoint + 1];

            ObjectState.WaypointMovingProgress += deltaTime * ObjectState.Speed;
            ObjectState.CurrentPosition = Vector3.Lerp(previous, next, ObjectState.WaypointMovingProgress);

            ObjectState.WaypointRotationProgress += deltaTime * ObjectState.AngularSpeed;
            var lookRotationVector = next - ObjectState.CurrentPosition;
            if (lookRotationVector != Vector3.zero)
            {
                var lookRotation = Quaternion.LookRotation(lookRotationVector);
                ObjectState.CurrentRotation = Quaternion.Slerp(ObjectState.CurrentRotation, lookRotation,
                    ObjectState.WaypointRotationProgress);
            }
        }

        private void FightStep(float deltaTime)
        {
            if (ObjectState.Target == null)
            {
                Debug.LogError("Enemy is null");
                return;
            }

            ObjectState.FightingTimeCollector += deltaTime;
            if (ObjectState.FightingTimeCollector > GameConstants.FightRoundDelay)
            {
                ObjectState.FightingTimeCollector = 0f;

                // execute fight
                var captured = ObjectState.Target.TryCapture(this);
                ObjectState.Power -= GameConstants.UnifiedDamage;

                if (ObjectState.Power <= 0)
                    Disband();

                if (captured)
                {
                    ObjectState.Status = ArmyStatus.Arriving;
                    ObjectState.Target = null;
                }
            }
        }

        public void HandlePlanetVisiting(Planet planet)
        {
            if (planet != ObjectState.Order.TargetPlanet)
                return;

            var opposite = ObjectState.Faction.GetOpposite();

            if (planet.Faction == ObjectState.Faction)
            {
                ObjectState.Status = ArmyStatus.Arriving;
            }
            else if (planet.Faction == Faction.Neutral || planet.Faction == opposite)
            {
                ObjectState.Status = ArmyStatus.Fighting;
                ObjectState.Target = planet;
            }
            else
            {
                Debug.LogError($"Unexpected planet faction {planet.Faction}");
            }
        }

        public void HandleCrash()
        {
            Disband();
        }

        private void Arrived()
        {
            ObjectState.Order.TargetPlanet.Reinforce(this);
            Disband();
        }

        private void Disband() => ObjectState.Status = ArmyStatus.Disbanded;

        protected override void OnClear(State state)
        {
            state.Order.Dispose();
        }
    }
}