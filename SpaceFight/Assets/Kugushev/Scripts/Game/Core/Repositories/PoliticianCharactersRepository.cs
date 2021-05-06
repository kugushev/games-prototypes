using System.Collections.Generic;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Repositories
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(PoliticianCharactersRepository))]
    public class PoliticianCharactersRepository : ScriptableObject
    {
        [SerializeField] private PoliticianCharacter[] characters = default!;

        public IReadOnlyList<PoliticianCharacter> Characters => characters;
    }
}