using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Common.Interfaces;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using Kugushev.Scripts.Game.Missions.Presets;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Entities
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
            var currentWaypoint = ObjectState.CurrentWaypoint;
            var path = ObjectState.Order.Path;

            if (path.Count <= currentWaypoint + 1)
            {
                Arrived();
                return;
            }

            var previous = path[currentWaypoint];
            var next = path[currentWaypoint + 1];
            
            ChangePosition();
            ChangeRotation();

            void ChangePosition()
            {
                var lookVector = next - previous;
                var newPosition = ObjectState.CurrentPosition + lookVector * (deltaTime * ObjectState.Speed);
                var dot = Vector3.Dot((next - newPosition).normalized, lookVector.normalized);
                if (dot < 0f)
                {
                    ObjectState.CurrentPosition = next;
                    
                    ObjectState.CurrentWaypoint++;
                    ObjectState.WaypointRotationProgress = 0f;
                }
                else
                    ObjectState.CurrentPosition = newPosition;
            }

            void ChangeRotation()
            {
                ObjectState.WaypointRotationProgress += deltaTime * ObjectState.AngularSpeed;
                var lookRotationVector = next - ObjectState.CurrentPosition;
                if (lookRotationVector != Vector3.zero)
                {
                    var lookRotation = Quaternion.LookRotation(lookRotationVector);
                    ObjectState.CurrentRotation = Quaternion.Slerp(ObjectState.CurrentRotation, lookRotation,
                        ObjectState.WaypointRotationProgress);
                }
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
                
                var captured = ExecuteFight();

                if (captured)
                {
                    ObjectState.Status = ArmyStatus.Arriving;
                    ObjectState.Target = null;
                }
            }

            bool ExecuteFight()
            {
                bool captured;
                if (ObjectState.Target.Faction != ObjectState.Faction)
                {
                    // execute fight
                    captured = ObjectState.Target.TryCapture(this);
                    ObjectState.Power -= GameConstants.UnifiedDamage;

                    if (ObjectState.Power <= 0)
                        Disband();
                }
                else
                    captured = true;

                return captured;
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