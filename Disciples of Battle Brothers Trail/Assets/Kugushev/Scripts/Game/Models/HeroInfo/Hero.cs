using System.Collections.Generic;
using Kugushev.Scripts.Game.Models.CityInfo;
using UniRx;

namespace Kugushev.Scripts.Game.Models.HeroInfo
{
    public class Hero
    {
        private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>(GameConstants.Hero.StartGold);
        private readonly ReactiveCollection<Teammate> _team = new ReactiveCollection<Teammate>();
        public static Hero Instance { get; } = new Hero();

        private Hero()
        {
        }

        public IReadOnlyReactiveProperty<int> Gold => _gold;

        public IReadOnlyReactiveCollection<Teammate> Team => _team;

        public bool TryHire(BattleUnit unit, int price, HiringDeskInfo owner)
        {
            if (_gold.Value >= price)
            {
                _gold.Value -= price;
                _team.Add(new Teammate(unit));

                owner.CommitBuying(unit);
                return true;
            }

            return false;
        }
    }
}