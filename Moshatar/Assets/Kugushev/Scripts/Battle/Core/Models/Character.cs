using UniRx;
using static Kugushev.Scripts.Battle.Core.GameConstants.Characters;

namespace Kugushev.Scripts.Battle.Core.Models
{
    public class Character
    {
        private readonly ReactiveProperty<int> _hp;

        public Character(int maxHp, int damage)
        {
            _hp = new ReactiveProperty<int>(maxHp);
            MaxHP = maxHp;
            Damage = damage;
        }

        public IReadOnlyReactiveProperty<int> HP => _hp;

        public int MaxHP { get; }
        public int Damage { get; }

        public void SufferDamage(int amount) => _hp.Value -= amount;
    }
}