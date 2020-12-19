using Kugushev.Scripts.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Components
{
    public class CharacterInteractable : MonoBehaviour
    {
        [SerializeField] private Character character;

        public Character Character => character;
    }
}