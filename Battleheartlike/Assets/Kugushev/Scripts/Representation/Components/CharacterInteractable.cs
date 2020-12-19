using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Representation.Components
{
    public class CharacterInteractable : MonoBehaviour
    {
        [SerializeField] private Character character;

        public Character Character => character;
    }
}