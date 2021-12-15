using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.ValueObjects.Parameters;

namespace Kugushev.Scripts.Battle.Core.Services
{
    public class BattleGameplayManager
    {
        public static BattleGameplayManager Instance { get; } = new BattleGameplayManager();
        
        public IBattleParameters Parameters { get; private set; } = new MusouBattleParameters();

        public void SetMusou() => Parameters = new MusouBattleParameters();

        public void SetTog() => Parameters = new TogBattleParameters();
    }
}