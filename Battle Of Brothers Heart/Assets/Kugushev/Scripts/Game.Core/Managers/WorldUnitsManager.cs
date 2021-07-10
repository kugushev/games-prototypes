using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Models.WorldUnits;
using UniRx;
using static Kugushev.Scripts.Game.Core.GameConstants.World;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class WorldUnitsManager
    {
        private readonly WorldManager _worldManager;
        private readonly ReactiveCollection<BanditWorldUnit> _bandits;

        public WorldUnitsManager(WorldManager worldManager)
        {
            _worldManager = worldManager;
            worldManager.WorldInitialized += CreateUnits;

            Player = new PlayerWorldUnit(
                GameConstants.Units.PlayerUnitStartPosition,
                GameConstants.Units.PlayerUnitStartDirection,
                new Party(new[]
                {
                    new Character(), new Character(), new Character()
                }));

            _bandits = new ReactiveCollection<BanditWorldUnit>();
        }

        public PlayerWorldUnit Player { get; }

        public IReadOnlyReactiveCollection<BanditWorldUnit> Bandits => _bandits;

        public void RemoveUnit(WorldUnit unit)
        {
            switch (unit)
            {
                case BanditWorldUnit banditWorldUnit:
                    _bandits.Remove(banditWorldUnit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit));
            }
        }

        private void CreateUnits()
        {
            foreach (var city in _worldManager.Cities)
            {
                int banditsCount = Random.Range(BanditsPerCityMin, BanditsPerCityMax + 1);

                for (int i = 0; i < banditsCount; i++)
                {
                    int power = Random.Range(BanditsPowerMin, BanditsPowerMax + 1);
                    var characters = Enumerable.Range(0, power).Select(_ => new Enemy()).ToArray();
                    var bandit = new BanditWorldUnit(city, characters);
                    _bandits.Add(bandit);
                }
            }
        }
    }
}