using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class Party
    {
        private readonly ReactiveCollection<Character> _characters;

        public Party(IReadOnlyList<Character> characters)
        {
            _characters = new ReactiveCollection<Character>(characters);
        }

        public IReadOnlyReactiveCollection<Character> Characters => _characters;

        public void Remove(Character character) => _characters.Remove(character);
        public void Add(Character character) => _characters.Add(character);
    }
}