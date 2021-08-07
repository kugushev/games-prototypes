using System;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class HitsManager
    {
        public AttackDirection Register(string weaponTag, int damage, Vector3[] line)
        {
            var from = line[0];
            var to = line[1];

            var direction = to - from;
            var projection = new Vector3(direction.x, 0, direction.z);

            var angle = Mathf.Acos(projection.magnitude / direction.magnitude);
            if (direction.y < 0)
                angle *= -1;

            var angleDeg = Mathf.FloorToInt(angle * Mathf.Rad2Deg);
            return Recognize(angleDeg);
        }

        private AttackDirection Recognize(int angle)
        {
            if (angle < -90 || angle > 90)
                Debug.LogError($"Unexpected angle {angle}");

            if (angle < -67.5f)
                return AttackDirection.VerticalDown;
            if (angle < -22.5f)
                return AttackDirection.DiagonalDown;
            if (angle < 22.5f)
                return AttackDirection.Horizontal;
            if (angle < 67.5f)
                return AttackDirection.DiagonalUp;
            return AttackDirection.VerticalUp;
        }
    }
}