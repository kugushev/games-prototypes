using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class BattleManager
    {
        private readonly WorldUnitsManager _worldUnitsManager;
        private readonly GameModeManager _gameModeManager;

        public BattleManager(WorldUnitsManager worldUnitsManager, GameModeManager gameModeManager)
        {
            _worldUnitsManager = worldUnitsManager;
            _gameModeManager = gameModeManager;
        }

        public BattleDefinition? CurrentBattle { get; private set; }

        public async void StartBattle(WorldUnit enemy)
        {
            CurrentBattle = new BattleDefinition(_worldUnitsManager.Player.Party, enemy.Party);
            await _gameModeManager.SwitchToBattleModeAsync();
        }
    }
}