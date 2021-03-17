using System;
using UnityEngine;

namespace Kugushev.Scripts.Common.ValueObjects
{
    [Serializable]
    public readonly struct Orbit
    {
        public const int DaysInYear = 360;
        private const int AllOrbitsAngle = 45;

        private readonly float _radius;
        private readonly Degree _alphaRotation;
        private readonly Degree _betaRotation;
        private readonly int _startDay;

        public Orbit(float radius, Degree alphaRotation, Degree betaRotation, int startDay)
        {
            _radius = radius;
            _alphaRotation = alphaRotation;
            _betaRotation = betaRotation;
            _startDay = startDay;
        }

        public Position ToPosition(Position sunPosition, int dayOfYear)
        {
            var angle = Mathf.Deg2Rad * (dayOfYear + _startDay);
            var smallRadius = _radius / 2;

            float x = sunPosition.Point.x + _radius * Mathf.Cos(angle);
            float y = sunPosition.Point.y + smallRadius * Mathf.Sin(angle);
            float z = sunPosition.Point.z;

            var flatPosition = new Vector3(x, y, z);

            var rotation = Quaternion.Euler(_alphaRotation.Value + AllOrbitsAngle, _betaRotation.Value, 0);

            Vector3 rotated = rotation * (flatPosition - sunPosition.Point) + sunPosition.Point;
            return new Position(rotated);
        }
    }
}