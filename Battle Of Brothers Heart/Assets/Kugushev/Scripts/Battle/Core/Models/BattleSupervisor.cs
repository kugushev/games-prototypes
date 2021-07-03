using System;
using System.Collections.Generic;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Managers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Core.Models
{
    public class BattleSupervisor : IFixedTickable
    {
        private readonly PlayerSquad _playerSquad;
        private readonly EnemySquad _enemySquad;
        private readonly BattleManager _battleManager;
        private readonly ReactiveProperty<TimeSpan?> _retreatElapsed = new ReactiveProperty<TimeSpan?>();
        private bool _battleFinished;

        public BattleSupervisor(PlayerSquad playerSquad, EnemySquad enemySquad, BattleManager battleManager)
        {
            _playerSquad = playerSquad;
            _enemySquad = enemySquad;
            _battleManager = battleManager;
        }

        public IReadOnlyReactiveProperty<TimeSpan?> RetreatElapsed => _retreatElapsed;

        public void ToggleRetreat(bool retreating)
        {
            if (retreating)
                _retreatElapsed.Value ??= BattleConstants.RetreatTime;
            else
                _retreatElapsed.Value = null;
        }

        void IFixedTickable.FixedTick()
        {
            if (_battleFinished)
                return;

            if (CheckRetreat())
                FinishBattle(BattleWinner.Enemy);

            if (IsWipe(_enemySquad.Units))
                FinishBattle(BattleWinner.Player);

            if (IsWipe(_playerSquad.Units))
                FinishBattle(BattleWinner.Enemy);
        }

        private void FinishBattle(BattleWinner result)
        {
            _battleFinished = true;
            _battleManager.FinishBattleAsync(result);
        }

        private bool CheckRetreat()
        {
            if (_retreatElapsed.Value != null)
            {
                _retreatElapsed.Value -= TimeSpan.FromSeconds(Time.fixedDeltaTime);
                if (_retreatElapsed.Value < TimeSpan.Zero)
                    return true;
            }

            return false;
        }

        private bool IsWipe(IEnumerable<BaseFighter> units)
        {
            bool atLeastOneUnitAlive = false;
            // todo: use for to avoid allocations
            foreach (var unit in units)
            {
                if (!unit.IsDead)
                {
                    atLeastOneUnitAlive = true;
                    break;
                }
            }

            return !atLeastOneUnitAlive;
        }
    }
}