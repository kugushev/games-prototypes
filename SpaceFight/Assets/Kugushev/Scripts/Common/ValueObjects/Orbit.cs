using System;
using UnityEngine;

namespace Kugushev.Scripts.Common.ValueObjects
{
    [Serializable]
    public readonly struct Orbit
    {
        private readonly Quaternion _rotation;
        private readonly float _radius;

        public Orbit(float radius, Quaternion rotation)
        {
            _rotation = rotation;
            _radius = radius;
        }

        public Position ToPosition(Position sunPosition, int dayOfYear)
        {
            var angle = Mathf.Deg2Rad * dayOfYear;
            float x = sunPosition.Point.x + _radius * Mathf.Cos(angle);
            float y = sunPosition.Point.y + _radius * Mathf.Sin(angle);
            float z = sunPosition.Point.z;

            var flatPosition = new Vector3(x, y, z);

            Vector3 rotated = _rotation * (flatPosition - sunPosition.Point) + sunPosition.Point;
            return new Position(rotated);
        }
    }
}