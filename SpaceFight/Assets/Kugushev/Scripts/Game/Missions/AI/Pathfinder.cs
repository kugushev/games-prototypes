using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Common;
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

        public float StepSize => stepSize;

        public bool FindPath<T>(Position from, Position to, Action<Position, T> filler, T objectToFill)
        {
            int length = 0;
            for (float i = 0; i < 1f; i += StepSize)
            {
                var point = Vector3.Lerp(from.Point, to.Point, i);
                filler(new Position(point), objectToFill);
                length++;
                if (length > maxLength)
                    return false;
            }

            return true;
        }
    }
}