using System.Collections.Generic;
using Kugushev.Scripts.Game.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Game.Managers
{
    public class CellsVisitingManager
    {
        private readonly Dictionary<Vector2Int, IInteractable> _visitors = new Dictionary<Vector2Int, IInteractable>();

        public IReadOnlyDictionary<Vector2Int, IInteractable> Visitors => _visitors;


        public void Register(Vector2Int cell, IInteractable interactable)
        {
            if (interactable == null)
            {
                Debug.Log("Interactable is null");
                return;
            }
            _visitors[cell] = interactable;
        }

        public void Unregister(Vector2Int cell, IInteractable interactable)
        {
            if (_visitors.TryGetValue(cell, out var current))
            {
                if (current != interactable)
                {
                    Debug.LogError($"Current cell is {current} but should be {interactable}");
                    return;
                }

                _visitors.Remove(cell);
            }
        }
    }
}