using System;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class City : IInteractable
    {
        public City(Position position)
        {
            Position = position;
            Name = Guid.NewGuid().ToString();
        }


        public Position Position { get; }

        public string Name { get; }

        bool IInteractable.IsInteractable => true;
    }
}