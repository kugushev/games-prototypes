using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Core.Exceptions;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class BattleManager
    {
        private readonly WorldUnitsManager _worldUnitsManager;
        private readonly GameModeManager _gameModeManager;
        private readonly Hero _hero;

        public BattleManager(WorldUnitsManager worldUnitsManager, GameModeManager gameModeManager, Hero hero)
        {
            _worldUnitsManager = worldUnitsManager;
            _gameModeManager = gameModeManager;
            _hero = hero;
        }

        public BattleDefinition? CurrentBattle { get; private set; }

        public BattleDefinition CurrentBattleSafe => CurrentBattle ?? throw new PropertyIsNotInitializedException();

        public async UniTask StartBattleAsync(WorldUnit enemy)
        {
            CurrentBattle = new BattleDefinition(_worldUnitsManager.Player, enemy);
            await _gameModeManager.SwitchToBattleModeAsync();
        }

        public async UniTask FinishBattleAsync(BattleWinner result)
        {
            if (CurrentBattle == null)
            {
                Debug.LogError("Current battle is null");
                return;
            }

            switch (result)
            {
                case BattleWinner.Player:
                    _hero.ApplyVictoryReward(CurrentBattle.Enemy.Party);
                    _worldUnitsManager.RemoveUnit(CurrentBattle.Enemy);
                    break;
                case BattleWinner.Enemy:
                    _hero.ApplyDefeat();
                    CurrentBattle.Enemy.Freeze(GameConstants.Units.WinFreezeDuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }

            await _gameModeManager.SwitchToCampaignModeAsync();
            CurrentBattle = null;
        }
    }
}