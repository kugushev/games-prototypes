using System.Linq;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.ProceduralGeneration;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.CityInfo
{
    public class HiringDeskInfo
    {
        private readonly ReactiveCollection<BattleUnit> _mercenaries = CreateMercenaries();

        public IReadOnlyReactiveCollection<BattleUnit> Mercenaries => _mercenaries;

        public void CommitBuying(BattleUnit unit)
        {
            var result = _mercenaries.Remove(unit);
            if (!result)
                Debug.LogError("Unit is not presented in list");
        }

        private static ReactiveCollection<BattleUnit> CreateMercenaries()
        {
            int lvl1Count = Random.Range(4, 5);
            int lvl2Count = Random.Range(1, 3);
            int lvl3Count = Random.Range(0, 1);

            var items = Enumerable.Range(0, lvl1Count).Select(_ => BattleUnitsGenerator.Create(1, GetAttackType()))
                .Concat(Enumerable.Range(0, lvl2Count).Select(_ => BattleUnitsGenerator.Create(2, GetAttackType())))
                .Concat(Enumerable.Range(0, lvl3Count).Select(_ => BattleUnitsGenerator.Create(3, GetAttackType())));

            return new ReactiveCollection<BattleUnit>(items);
        }

        private static AttackType GetAttackType() => (AttackType) Random.Range(1, 3);
    }
}