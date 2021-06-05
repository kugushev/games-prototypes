using System;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.Models
{
    public class EnemyUnit
    {
        private readonly ReactiveProperty<Position> _position =
            new ReactiveProperty<Position>(new Position(new Vector2(8, 0)));

        public IReadOnlyReactiveProperty<Position> Position => _position;

        public event Action? Hurt;

        public void Suffer()
        {
            Hurt?.Invoke();
        }
    }
}