using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Repositories
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(PoliticianCharactersRepository))]
    public class PoliticianCharactersRepository : ScriptableObject
    {
        [SerializeField] private PoliticianCharacter[] characters = default!;

        public IReadOnlyList<PoliticianCharacter> Characters => characters;
    }
}