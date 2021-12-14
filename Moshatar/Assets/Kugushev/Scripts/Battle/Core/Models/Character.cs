using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Models
{
    public class Character
    {
        private readonly ReactiveProperty<int> _hp;

        public Character(int maxHp, int damage, float attackRange = BattleConstants.SwordAttackRange)
        {
            _hp = new ReactiveProperty<int>(maxHp);
            MaxHP = maxHp;
            Damage = damage;
            AttackRange = attackRange;
        }

        public IReadOnlyReactiveProperty<int> HP => _hp;

        public int MaxHP { get; }
        public int Damage { get; }
        public float AttackRange { get; }

        public void SufferDamage(int amount) => _hp.Value -= amount;
        public void Regenerate(int amount) => _hp.Value = Mathf.Min(_hp.Value + amount, MaxHP);
    }
}