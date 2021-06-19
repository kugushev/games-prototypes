using Kugushev.Scripts.Game.Core.Interfaces;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models
{
    public class City : IInteractable
    {
        public City(Vector2Int position)
        {
            Position = position;
        }

        public Vector2Int Position { get; }

        bool IInteractable.IsInteractable => true;
        Position IInteractable.Position => new Position(Position);
    }
}