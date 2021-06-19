using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Models
{
    public class City
    {
        public Vector2Int Position { get; }

        public City(Vector2Int position)
        {
            Position = position;
        }
    }
}