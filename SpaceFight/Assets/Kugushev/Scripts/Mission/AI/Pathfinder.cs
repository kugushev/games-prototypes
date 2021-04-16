using System;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.AI
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Pathfinder")]
    public class Pathfinder : ScriptableObject
    {
        [SerializeField] private MissionModelProvider? missionModelProvider;
        [SerializeField] private float stepSize = GameplayConstants.GapBetweenWaypoints;
        [SerializeField] private int maxLength = GameplayConstants.OrderPathCapacity;
        [SerializeField] private float collisionError = GameplayConstants.CollisionError;

        public float StepSize => stepSize;

        public bool FindPathLength(Position from, Position to, float hitRadius, out int length) =>
            FindPath<object>(from, to, hitRadius, null, default, out length);

        public bool FindPath<T>(Position from, Position to, float hitRadius, Action<Position, T>? filler,
            T objectToFill) where T : class =>
            FindPath(from, to, hitRadius, filler, objectToFill, out _);

        public bool FindPath<T>(Position from, Position to, float hitRadius, Action<Position, T>? filler,
            T? objectToFill, out int length) where T : class
        {
            Asserting.NotNull(missionModelProvider);

            if (!missionModelProvider.TryGetModel(out var missionModel))
            {
                length = 0;
                return false;
            }

            ref readonly var sun = ref missionModel.PlanetarySystem.GetSun();

            length = 0;
            var previous = from.Point;

            while (Vector3.Distance(to.Point, previous) > StepSize)
            {
                var lookVector = (to.Point - previous).normalized;
                var point = previous + lookVector * StepSize;

                point = AvoidSun(hitRadius, sun, point);

                if (objectToFill is { } && filler is { })
                    filler(new Position(point), objectToFill);

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