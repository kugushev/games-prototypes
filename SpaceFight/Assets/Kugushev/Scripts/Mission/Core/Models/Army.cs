using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;
using Zenject;
using Order = Kugushev.Scripts.Mission.Core.ValueObjects.Order;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public class Army : IFighter, IPoolable<Order, Faction, Power, IFleetEffects, IMemoryPool>, IDisposable
    {
        private readonly IPlanetarySystem _planetarySystem;
        private readonly EventsCollectingService _eventsCollectingService;

        private readonly List<IFighter> _targets = new List<IFighter>();
        private readonly List<IFighter> _targetsToRemoveBuffer = new List<IFighter>(8);
        private State _state;

        private struct State
        {
            public readonly Order Order;
            public Faction Faction;
            public float Power;
            public ArmyStatus Status;
            public Vector3 CurrentPosition;
            public Quaternion CurrentRotation;
            public int CurrentWaypoint;
            public float WaypointRotationProgress;
            public float FightingTimeCollector;

            public bool NearDeath;
            public readonly float StartPower;
            public IFleetEffects FleetEffects { get; }
            public bool Initialized { get; }


            public State(Order order, Faction faction, float power, IFleetEffects fleetEffects)
            {
                this.Order = order;
                this.Faction = faction;
                this.Power = power;
                FleetEffects = fleetEffects;
                Status = ArmyStatus.Recruiting;
                CurrentPosition = order.Path[0];
                CurrentRotation = Quaternion.identity;
                CurrentWaypoint = 0;
                WaypointRotationProgress = 0f;
                FightingTimeCollector = 0f;
                NearDeath = false;
                StartPower = power;
                Initialized = true;
            }
        }

        public Army(IPlanetarySystem planetarySystem, EventsCollectingService eventsCollectingService)
        {
            _planetarySystem = planetarySystem;
            _eventsCollectingService = eventsCollectingService;
        }

        #region IPoolable, IDisposable

        private IMemoryPool? _pool;

        void IPoolable<Order, Faction, Power, IFleetEffects, IMemoryPool>.OnSpawned(Order p1, Faction p2, Power p3,
            IFleetEffects p4, IMemoryPool p5)
        {
            _state = new State(p1, p2, p3.Amount, p4);
            _pool = p5;
        }

        void IPoolable<Order, Faction, Power, IFleetEffects, IMemoryPool>.OnDespawned()
        {
            _state = default;
            _pool = null;
        }

        public void Dispose() => _pool?.Despawn(this);

        public class Factory : PlaceholderFactory<Order, Faction, Power, Army>
        {
        }

        #endregion

        public bool Active => _state.Initialized;

        public ArmyStatus Status
        {
            get => _state.Status;
            set => _state.Status = value;
        }

        public Position Position => new Position(_state.CurrentPosition);
        public Quaternion Rotation => _state.CurrentRotation;
        public float Power => _state.Power;
        public bool Disbanded => _state.Status == ArmyStatus.Disbanded;
        public Faction Faction => _state.Faction;

        public bool CanBeAttacked => _state.Status != ArmyStatus.Arriving &&
                                     _state.Status != ArmyStatus.Disbanded;

        public IEnumerable<IFighter> GetTargetsUnderFire(int targets = GameplayConstants.ArmyCannonsCount) =>
            _targets.Take(targets);

        public void NextStep(float deltaTime)
        {
            if (!Active)
                return;

            switch (_state.Status)
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
            var currentWaypoint = _state.CurrentWaypoint;
            var path = _state.Order.Path;

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
                var newPosition = _state.CurrentPosition + lookVector * (deltaTime * CalcSpeed(lookVector));
                var dot = Vector3.Dot((next - newPosition).normalized, lookVector);
                if (dot <= 0f || _state.CurrentPosition == next)
                {
                    _state.CurrentPosition = next;

                    _state.CurrentWaypoint++;
                    _state.WaypointRotationProgress = 0f;
                }
                else
                    _state.CurrentPosition = newPosition;
            }

            void ChangeRotation()
            {
                _state.WaypointRotationProgress += deltaTime * GameplayConstants.ArmyAngularSpeed;
                var lookRotationVector = next - _state.CurrentPosition;
                if (lookRotationVector != Vector3.zero)
                {
                    var lookRotation = Quaternion.LookRotation(lookRotationVector);
                    _state.CurrentRotation = Quaternion.Slerp(_state.CurrentRotation, lookRotation,
                        _state.WaypointRotationProgress);
                }
            }
        }

        float CalcSpeed(Vector3 lookVector)
        {
            var sunMultiplier = GetSunMultiplier();

            var speed = _state.FleetEffects.ArmySpeed.Calculate(GameplayConstants.ArmySpeed,
                (_state.Order.TargetPlanet, Faction));

            return speed * sunMultiplier;

            float GetSunMultiplier()
            {
                var sunFallVector = (_state.CurrentPosition - _planetarySystem.Sun.Position.Point).normalized;
                lookVector = lookVector.normalized;

                var dot = Vector3.Dot(sunFallVector, lookVector);

                float multiplier;
                if (dot >= 0.5f) // less than 60 deg
                    multiplier = GameplayConstants.SunPowerSpeedMultiplier;
                else if (dot > -0.5) // between 60 and 120 deg
                    multiplier = 1;
                else // more than 120 deg
                    multiplier = 1 / GameplayConstants.SunPowerSpeedMultiplier;
                return multiplier;
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

            _state.FightingTimeCollector += deltaTime;
            if (_state.FightingTimeCollector > GameplayConstants.SiegeRoundDelay)
            {
                _state.FightingTimeCollector = 0f;

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
                if (target.Faction.Value != _state.Faction)
                {
                    // consider ultimatum
                    if (target.Faction.Value == Faction.Neutral)
                    {
                        var ultimatum = _state.FleetEffects.ToNeutralPlanetUltimatum;
                        if (ultimatum.Initialized)
                        {
                            captured = target.Consider(in ultimatum, this);
                            if (captured)
                                return true;
                        }
                    }

                    // execute siege
                    float damage = _state.FleetEffects.SiegeDamage.Calculate(GameplayConstants.UnifiedDamage, this);

                    var result = target.SufferFightRound(Faction, damage, this);
                    captured = result == FightRoundResult.Defeated;

                    if (!captured)
                        SufferFromPlanet(target);
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

            _state.FightingTimeCollector += deltaTime;
            if (force || _state.FightingTimeCollector > GameplayConstants.FightRoundDelay)
            {
                _state.FightingTimeCollector = 0f;

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
                            _eventsCollectingService.ArmyDestroyedInFight.Add(
                                new ArmyDestroyedInFight(_state.Faction, targetArmy.Faction, _state.Power));

                            _targetsToRemoveBuffer.Add(targetArmy);
                        }
                    }
                }
            }

            RemoveTargetsToRemove();

            if (_targets.Count == 0)
                _state.Status = ArmyStatus.OnMatch;

            bool ExecuteFight(Army targetArmy)
            {
                if (targetArmy.Faction == Faction)
                {
                    Debug.LogError($"We're trying to hit our allies {_state.Faction}");
                    return false;
                }

                var fleetEffects = _state.FleetEffects;

                float damage = fleetEffects.FightDamage.Calculate(GameplayConstants.UnifiedDamage, this);

                if (_state.NearDeath && fleetEffects.DeathStrike > 0)
                    damage = fleetEffects.DeathStrike;

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
            damage = _state.FleetEffects.FightProtection.Calculate(damage, this);

            var result = ExecuteSuffer(enemyFaction, damage);

            if (result == FightRoundResult.Defeated && _state.FleetEffects.DeathStrike > 0)
            {
                _state.NearDeath = true;
                FightStep(GameplayConstants.FightRoundDelay, true, 1);
            }

            return result;
        }

        private FightRoundResult SufferFromPlanet(Planet enemy)
        {
            var damage = enemy.GetDamage();

            var result = ExecuteSuffer(enemy.Faction.Value, damage);

            if (result == FightRoundResult.Defeated)
            {
                _eventsCollectingService.ArmyDestroyedOnSiege.Add(new ArmyDestroyedOnSiege(
                    enemy.Faction.Value, Faction, _state.StartPower, enemy.Power.Value));
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

            _state.Power -= damage;
            if (_state.Power <= 0)
            {
                Disband();
                return FightRoundResult.Defeated;
            }

            return FightRoundResult.StillAlive;
        }

        #endregion


        public void HandlePlanetVisiting(Planet planet)
        {
            if (_state.Status != ArmyStatus.OnMatch)
                return;

            if (planet != _state.Order.TargetPlanet)
                return;

            var opposite = _state.Faction.GetOpposite();

            if (planet.Faction.Value == _state.Faction)
            {
                Arrive();
            }
            else if (planet.Faction.Value == Faction.Neutral || planet.Faction.Value == opposite)
            {
                _state.Status = ArmyStatus.OnSiege;
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
            if (_state.Status != ArmyStatus.OnMatch && _state.Status != ArmyStatus.Fighting)
                return;

            if (otherPartyArmy.Faction == _state.Faction)
                return;

            if (!otherPartyArmy.CanBeAttacked)
                return;

            _state.Status = ArmyStatus.Fighting;

            if (!_targets.Contains(otherPartyArmy))
                _targets.Add(otherPartyArmy);
            else
                Debug.LogWarning("Other army is already in targets");
        }

        private void Arrive()
        {
            var armyArrived = new ArmyArrived(Faction, Power);

            _state.Status = ArmyStatus.Arriving;
            _state.Order.TargetPlanet.Reinforce(this);

            _eventsCollectingService.ArmyArrived.Add(armyArrived);
        }

        private void Disband() => _state.Status = ArmyStatus.Disbanded;
    }
}