using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models.WorldUnits;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class Hero : IInitializable
    {
        private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>();
        private readonly WorldUnit _unit;

        public Hero(WorldUnitsManager worldUnitsManager)
        {
            _unit = worldUnitsManager.Player;
        }

        void IInitializable.Initialize()
        {
            // todo: load hero data from save
        }

        public IReadOnlyReactiveProperty<int> Gold => _gold;

        public void ApplyVictoryReward(Party defeated)
        {
            var income = defeated.Characters.Count switch
            {
                1 => 2,
                2 => 3,
                3 => 5,
                4 => 8,
                5 => 13,
                6 => 21,
                7 => 34,
                8 => 55,
                9 => 89,
                10 => 144,
                _ => throw new Exception($"Unexpected defeated count {defeated.Characters.Count}")
            };

            _gold.Value += income;
        }

        public void ApplyDefeat()
        {
            _gold.Value -= _gold.Value / 10;
            RemoveAllDownedCharacters();

            void RemoveAllDownedCharacters()
            {
                var dead = new List<Character>();
                foreach (var character in _unit.Party.Characters)
                    if (character.HP.Value <= 0)
                        dead.Add(character);

                foreach (var character in dead)
                    _unit.Party.Remove(character);
            }
        }
    }
}