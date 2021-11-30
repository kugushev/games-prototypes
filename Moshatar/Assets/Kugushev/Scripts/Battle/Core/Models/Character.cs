using UniRx;
using static Kugushev.Scripts.Battle.Core.GameConstants.Characters;

namespace Kugushev.Scripts.Battle.Core.Models
{
    public class Character
    {
        private readonly ReactiveProperty<int> _hp = new ReactiveProperty<int>(DefaultMaxHitPoints);

        private readonly ReactiveProperty<int> _maxHP = new ReactiveProperty<int>(DefaultMaxHitPoints);
        
        public IReadOnlyReactiveProperty<int> HP => _hp;

        public IReadOnlyReactiveProperty<int> MaxHP => _maxHP;

        public void SufferDamage(int amount) => _hp.Value -= amount;
    }
}