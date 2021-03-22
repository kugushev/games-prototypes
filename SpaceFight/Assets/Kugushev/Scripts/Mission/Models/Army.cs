using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [Serializable]
    public class Army : Poolable<Army.State>, IGameLoopParticipant, IFighter
    {
        [Serializable]
        public struct State
        {
            public Order order;
            public float speed;
            public float angularSpeed;
            public Faction faction;
            public float power;
            public Sun sun;
            public FleetPerks FleetPerks;
            public ArmyStatus status;
            public Vector3 currentPosition;
            public Quaternion currentRotation;
            public int currentWaypoint;
            public float waypointRotationProgress;
            public float fightingTimeCollector;
            public readonly MissionEventsCollector EventsCollector;
            public bool nearDeath;

            public State(Order order, float speed, float angularSpeed, Faction faction, float power, in Sun sun,
                in FleetPerks fleetPerks, MissionEventsCollector eventsCollector)
            {
                this.order = order;
                this.speed = speed;
                this.angularSpeed = angularSpeed;
                this.faction = faction;
                this.power = power;
                this.sun = sun;
                FleetPerks = fleetPerks;
                EventsCollector = eventsCollector;
                status = ArmyStatus.Recruiting;
                currentPosition = order.Path[0];
                currentRotation = Quaternion.identity;
                currentWaypoint = 0;
                waypointRotationProgress = 0f;
                fightingTimeCollector = 0f;
                nearDeath = false;
            }
        }

        private readonly List<IFighter> _targets = new List<IFighter>();
        private readonly List<IFighter> _targetsToRemoveBuffer = new List<IFighter>(8);

        public Army(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public ArmyStatus Status
        {
            get => ObjectState.status;
            set => ObjectState.status = value;
        }

        public Position Position => new Position(ObjectState.currentPosition);
        public Quaternion Rotation => ObjectState.currentRotation;
        public float Power => ObjectState.power;
        public bool Disbanded => ObjectState.status == ArmyStatus.Disbanded;
        public Faction Faction => ObjectState.faction;

        public bool CanBeAttacked => ObjectState.status != ArmyStatus.Arriving &&
                                     ObjectState.status != ArmyStatus.Disbanded;

        public IEnumerable<IFighter> GetTargetsUnderFire(int targets = GameplayConstants.ArmyCannonsCount) =>
            _targets.Take(targets);

        public void NextStep(float deltaTime)
        {
            if (!Active)
                return;

            switch (ObjectState.status)
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
            var currentWaypoint = ObjectState.currentWaypoint;
            var path = ObjectState.order.Path;

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
                var newPosition = ObjectState.currentPosition + lookVector * (deltaTime * CalcSpeed(lookVector));
                var dot = Vector3.Dot((next - newPosition).normalized, lookVector);
                if (dot <= 0f || ObjectState.currentPosition == next)
                {
                    ObjectState.currentPosition = next;

                    ObjectState.currentWaypoint++;
                    ObjectState.waypointRotationProgress = 0f;
                }
                else
                    ObjectState.currentPosition = newPosition;
            }

            void ChangeRotation()
            {
                ObjectState.waypointRotationProgress += deltaTime * ObjectState.angularSpeed;
                var lookRotationVector = next - ObjectState.currentPosition;
                if (lookRotationVector != Vector3.zero)
                {
                    var lookRotation = Quaternion.LookRotation(lookRotationVector);
                    ObjectState.currentRotation = Quaternion.Slerp(ObjectState.currentRotation, lookRotation,
                        ObjectState.waypointRotationProgress);
                }
            }

            float CalcSpeed(Vector3 lookVector)
            {
                var sunFallVector = (ObjectState.currentPosition - ObjectState.sun.Position.Point).normalized;
                lookVector = lookVector.normalized;

                var dot = Vector3.Dot(sunFallVector, lookVector);

                float multiplication;
                if (dot >= 0.5f) // less than 60 deg
                    multiplication = GameplayConstants.SunPowerSpeedMultiplier;
                else if (dot > -0.5) // between 60 and 120 deg
                    multiplication = 1;
                else // more than 120 deg
                    multiplication = 1 / GameplayConstants.SunPowerSpeedMultiplier;

                return ObjectState.speed * multiplication;
            }
        }

        private void SiegeStep(float deltaTime)
        {
            if (_targets.Count == 0)
            {
                Debug.LogError("No enemies");
                return;
            }

            _targetsToRemoveBuffer.Clear();

            ObjectState.fightingTimeCollector += deltaTime;
            if (ObjectState.fightingTimeCollector > GameplayConstants.SiegeRoundDelay)
            {
                ObjectState.fightingTimeCollector = 0f;

                foreach (var target in GetTargetsUnderFire())
                    if (target is Planet targetPlanet)
                    {
                        bool captured = !target.CanBeAttacked;

                        if (!captured)
                            captured = ExecuteSiege(targetPlanet);

                        if (captured)
                            _targetsToRemoveBuffer.Add(target);
                    }
            }

            RemoveTargetsToRemove();
            if (_targets.Count == 0)
                Arrive();

            bool ExecuteSiege(Planet target)
            {
                bool captured;
                if (target.Faction != ObjectState.faction)
                {
                    // execute siege
                    float damage = ObjectState.FleetPerks.SiegeDamage.Calculate(GameplayConstants.UnifiedDamage, this);

                    var result = target.SufferFightRound(Faction, damage);
                    captured = result == FightRoundResult.Defeated;

                    if (!captured)
                        ExecuteSuffer(target.Faction);
                }
                else
                    captured = true;

                return captured;
            }
        }

        private void FightStep(float deltaTime, bool force = false, int targets = GameplayConstants.ArmyCannonsCount)
        {
            if (_targets.Count == 0)
            {
                Debug.LogError("No enemies");
                return;
            }

            _targetsToRemoveBuffer.Clear();

            ObjectState.fightingTimeCollector += deltaTime;
            if (force || ObjectState.fightingTimeCollector > GameplayConstants.FightRoundDelay)
            {
                ObjectState.fightingTimeCollector = 0f;

                foreach (var target in GetTargetsUnderFire(targets))
                {
                    if (target is Army targetArmy)
                    {
                        if (!targetArmy.Active)
                        {
                            Debug.LogWarning("Enemy is not active");
                            _targetsToRemoveBuffer.Add(target);
                            continue;
                        }

                        bool enemyIsDefeated = !targetArmy.CanBeAttacked;

                        if (!enemyIsDefeated)
                            enemyIsDefeated = ExecuteFight(targetArmy);

                        if (enemyIsDefeated)
                        {
                            ObjectState.EventsCollector.ArmyDestroyedInFight.Add(
                                new ArmyDestroyedInFight(ObjectState.faction, targetArmy.Faction, ObjectState.power));

                            _targetsToRemoveBuffer.Add(targetArmy);
                        }
                    }
                }
            }

            RemoveTargetsToRemove();

            if (_targets.Count == 0)
                ObjectState.status = ArmyStatus.OnMatch;

            bool ExecuteFight(Army targetArmy)
            {
                if (targetArmy.Faction == Faction)
                {
                    Debug.LogError($"We're trying to hit our allies {ObjectState.faction}");
                    return false;
                }

                var fleetPerks = ObjectState.FleetPerks;

                float damage = fleetPerks.FightDamage.Calculate(GameplayConstants.UnifiedDamage, this);

                if (ObjectState.nearDeath && fleetPerks.DeathStrike > 0)
                    damage = fleetPerks.DeathStrike;

                var result = targetArmy.SufferFightRound(Faction, damage);
                return result == FightRoundResult.Defeated;
            }
        }

        private void RemoveTargetsToRemove()
        {
            foreach (var targetToRemove in _targetsToRemoveBuffer) _targets.Remove(targetToRemove);
            _targetsToRemoveBuffer.Clear();
        }

        #region IFighter

        public FightRoundResult SufferFightRound(Faction enemyFaction, float damage = GameplayConstants.UnifiedDamage)
        {
            damage = ObjectState.FleetPerks.FightProtection.Calculate(damage, this);

            var result = ExecuteSuffer(enemyFaction, damage);

            if (result == FightRoundResult.Defeated && ObjectState.FleetPerks.DeathStrike > 0)
            {
                ObjectState.nearDeath = true;
                FightStep(GameplayConstants.FightRoundDelay, true, 1);
            }

            return result;
        }

        private FightRoundResult ExecuteSuffer(Faction enemyFaction, float damage = GameplayConstants.UnifiedDamage)
        {
            if (enemyFaction == Faction)
            {
                Debug.LogError($"Ally is trying to kill me {enemyFaction}");
                return FightRoundResult.StillAlive;
            }

            ObjectState.power -= damage;
            if (ObjectState.power <= 0)
            {
                Disband();
                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        #endregion


        public void HandlePlanetVisiting(Planet planet)
        {
            if (ObjectState.status != ArmyStatus.OnMatch)
                return;

            if (planet != ObjectState.order.TargetPlanet)
                return;

            var opposite = ObjectState.faction.GetOpposite();

            if (planet.Faction == ObjectState.faction)
            {
                Arrive();
            }
            else if (planet.Faction == Faction.Neutral || planet.Faction == opposite)
            {
                ObjectState.status = ArmyStatus.OnSiege;
                if (!_targets.Contains(planet))
                    _targets.Add(planet);
                else
                    Debug.LogWarning("Planet is already in targets");
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

        public void HandleArmyInteraction(IFighter otherPartyArmy)
        {
            if (ObjectState.status != ArmyStatus.OnMatch && ObjectState.status != ArmyStatus.Fighting)
                return;

            if (otherPartyArmy.Faction == ObjectState.faction)
                return;

            if (!otherPartyArmy.CanBeAttacked)
                return;

            ObjectState.status = ArmyStatus.Fighting;

            if (!_targets.Contains(otherPartyArmy))
                _targets.Add(otherPartyArmy);
            else
                Debug.LogWarning("Other army is already in targets");
        }

        private void Arrive()
        {
            ObjectState.status = ArmyStatus.Arriving;
            ObjectState.order.TargetPlanet.Reinforce(this);
        }

        private void Disband() => ObjectState.status = ArmyStatus.Disbanded;

        protected override void OnRestore(State state) => _targets.Clear();

        protected override void OnClear(State state)
        {
            state.order.Dispose();
            _targets.Clear();
        }
    }
}