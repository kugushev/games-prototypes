using System;
using JetBrains.Annotations;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.AI
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Pathfinder")]
    public class Pathfinder : ScriptableObject
    {
        [SerializeField] private MissionManager missionManager;
        [SerializeField] private float stepSize = GameConstants.GapBetweenWaypoints;
        [SerializeField] private int maxLength = GameConstants.OrderPathCapacity;
        [SerializeField] private float collisionError = GameConstants.CollisionError;

        public float StepSize => stepSize;

        public bool FindPath(Position from, Position to, float hitRadius, out int length) =>
            FindPath<object>(from, to, hitRadius, null, default, out length);
        
        public bool FindPath<T>(Position from, Position to, float hitRadius, [CanBeNull] Action<Position, T> filler,
            T objectToFill) =>
            FindPath(from, to, hitRadius, filler, objectToFill, out _);

        public bool FindPath<T>(Position from, Position to, float hitRadius, [CanBeNull] Action<Position, T> filler,
            T objectToFill, out int length)
        {
            if (missionManager.State == null)
            {
                length = 0;
                return false;
            }

            ref readonly var sun = ref missionManager.State.Value.CurrentPlanetarySystem.GetSun();

            length = 0;
            var previous = from.Point;

            while (Vector3.Distance(to.Point, previous) > StepSize)
            {
                var lookVector = (to.Point - previous).normalized;
                var point = previous + lookVector * StepSize;

                point = AvoidSun(hitRadius, sun, point);

                filler?.Invoke(new Position(point), objectToFill);

                previous = point;
                length++;

                if (length > maxLength)
                    return false;
            }
            
            return true;
        }

        private Vector3 AvoidSun(float hitRadius, Sun sun, Vector3 point)
        {
            var distanceToSun = Vector3.Distance(sun.Position.Point, point);
            var avoidanceStep = sun.Radius + hitRadius + collisionError - distanceToSun;
            if (avoidanceStep > 0)
            {
                var fromSunToPoint = (point - sun.Position.Point).normalized;
                point += fromSunToPoint * avoidanceStep;
            }

            return point;
        }
    }
}