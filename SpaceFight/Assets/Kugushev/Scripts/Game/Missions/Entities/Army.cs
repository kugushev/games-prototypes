using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Common.Interfaces;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using Kugushev.Scripts.Game.Missions.Presets;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Entities
{
    public class Army : Poolable<Army.State>, IGameLoopParticipant, IFighter
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

        public Position Position => new Position(ObjectState.CurrentPosition);
        public Quaternion Rotation => ObjectState.CurrentRotation;
        public int Power => ObjectState.Power;
        public bool Disbanded => ObjectState.Status == ArmyStatus.Disbanded;
        public Faction Faction => ObjectState.Faction;
        public bool CanBeAttacked => ObjectState.Status != ArmyStatus.Arriving &&
                                     ObjectState.Status != ArmyStatus.Disbanded;

        public IFighter CurrentTarget => ObjectState.Target;

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
                case ArmyStatus.OnSiege:
                    SiegeStep(deltaTime);
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
                Disband();
                return;
            }

            var previous = path[currentWaypoint];
            var next = path[currentWaypoint + 1];

            ChangePosition();
            ChangeRotation();

            void ChangePosition()
            {
                var lookVector = (next - previous).normalized;
                var newPosition = ObjectState.CurrentPosition + lookVector * (deltaTime * ObjectState.Speed);
                var dot = Vector3.Dot((next - newPosition).normalized, lookVector);
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

        private void SiegeStep(float deltaTime)
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

                bool captured = !ObjectState.Target.CanBeAttacked;

                if (!captured)
                    captured = ExecuteSiege();

                if (captured)
                {
                    Arrive();
                    ObjectState.Target = null;
                }
            }

            bool ExecuteSiege()
            {
                bool captured;
                if (ObjectState.Target.Faction != ObjectState.Faction)
                {
                    // execute fight
                    var result = ObjectState.Target.SufferFightRound(Faction);
                    captured = result == FightRoundResult.Defeated;

                    if (!captured)
                        SufferFightRound(ObjectState.Target.Faction);
                }
                else
                    captured = true;

                return captured;
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

                bool enemyIsDefeated = !ObjectState.Target.CanBeAttacked;

                if (!enemyIsDefeated)
                    enemyIsDefeated = ExecuteFight();

                if (enemyIsDefeated)
                {
                    ObjectState.Status = ArmyStatus.OnMatch;
                    ObjectState.Target = null;
                }
            }

            bool ExecuteFight()
            {
                if (ObjectState.Target.Faction == Faction)
                {
                    Debug.LogError($"We're trying to hit our allies {ObjectState.Faction}");
                    return false;
                }

                var result = ObjectState.Target.SufferFightRound(Faction);
                return result == FightRoundResult.Defeated;
            }
        }

        #region IFighter

        public FightRoundResult SufferFightRound(Faction enemyFaction, int damage = GameConstants.UnifiedDamage)
        {
            if (enemyFaction == Faction)
            {
                Debug.LogError($"Ally is trying to kill me {enemyFaction}");
                return FightRoundResult.StillAlive;
            }

            ObjectState.Power -= damage;
            if (ObjectState.Power <= 0)
            {
                Disband();
                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        #endregion


        public void HandlePlanetVisiting(Planet planet)
        {
            if (ObjectState.Status != ArmyStatus.OnMatch)
                return;

            if (planet != ObjectState.Order.TargetPlanet)
                return;

            var opposite = ObjectState.Faction.GetOpposite();

            if (planet.Faction == ObjectState.Faction)
            {
                Arrive();
            }
            else if (planet.Faction == Faction.Neutral || planet.Faction == opposite)
            {
                ObjectState.Status = ArmyStatus.OnSiege;
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


        public void HandleArmyInteraction(Army otherPartyArmy)
        {
            if (ObjectState.Status != ArmyStatus.OnMatch)
                return;

            if (otherPartyArmy.Faction == ObjectState.Faction)
                return;

            if (!otherPartyArmy.CanBeAttacked)
                return;

            ObjectState.Status = ArmyStatus.Fighting;
            ObjectState.Target = otherPartyArmy;
        }

        private void Arrive()
        {
            ObjectState.Status = ArmyStatus.Arriving;
            ObjectState.Order.TargetPlanet.Reinforce(this);
        }

        private void Disband() => ObjectState.Status = ArmyStatus.Disbanded;

        protected override void OnClear(State state)
        {
            state.Order.Dispose();
        }
    }
}