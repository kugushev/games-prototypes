using System.Linq;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.ProceduralGeneration;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Units
{
    public class BanditModel: IInteractable
    {
        private const int MinSquadSize = 5;
        private const int MaxSquadSize = 10;

        private readonly ReactiveCollection<BattleUnit> _squad;

        public BanditModel()
        {
            int size = Random.Range(MinSquadSize, MaxSquadSize);
            _squad = new ReactiveCollection<BattleUnit>(Enumerable.Range(0, size)
                .Select(_ => BattleUnitsGenerator.Create(1, AttackType.Mele)));
        }

        public IReadOnlyReactiveCollection<BattleUnit> Squad => _squad;
    }
}