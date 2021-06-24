using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.City.Core.Models
{
    public class RoadSign : IInteractable
    {
        public RoadSign()
        {
            Position = new Position(new Vector2(-9, -3));
        }

        public Position Position { get; }
        public bool IsInteractable => true;
    }
}