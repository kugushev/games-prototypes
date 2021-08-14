using System.Collections.Generic;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Models.CityInfo;
using UniRx;

namespace Kugushev.Scripts.Game.Models.HeroInfo
{
    public class Hero: IInteractable
    {
        private const int MaxTeamSize = 4;
        private const int StartGold = 300;

        private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>(StartGold);
        private readonly ReactiveCollection<Teammate> _team = new ReactiveCollection<Teammate>();

        private Hero()
        {
        }

        public IReadOnlyReactiveProperty<int> Gold => _gold;

        public IReadOnlyReactiveCollection<Teammate> Team => _team;

        public bool TryHire(BattleUnit unit, int price, HiringDeskInfo owner)
        {
            if (_team.Count >= MaxTeamSize)
                return false;

            if (_gold.Value >= price)
            {
                _gold.Value -= price;
                _team.Add(new Teammate(unit));

                owner.CommitBuying(unit);
                return true;
            }

            return false;
        }

        public void Fire(Teammate teammate) => _team.Remove(teammate);
    }
}