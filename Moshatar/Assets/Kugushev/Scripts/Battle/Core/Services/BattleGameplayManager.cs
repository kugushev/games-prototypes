using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.ValueObjects.Parameters;

namespace Kugushev.Scripts.Battle.Core.Services
{
    public class BattleGameplayManager
    {
        public static BattleGameplayManager Instance { get; } = new BattleGameplayManager();
        
        // public IBattleParameters Parameters { get; private set; } = new MusouBattleParameters();
        // public Mode SeletedMode { get; private set; } = Mode.Musou;
        
        public IBattleParameters Parameters { get; private set; } = new TogBattleParameters();
        public Mode SeletedMode { get; private set; } = Mode.Tog;

        public void SetMusou()
        {
            Parameters = new MusouBattleParameters();
            SeletedMode = Mode.Musou;
        }

        public void SetTog()
        {
            Parameters = new TogBattleParameters();
            SeletedMode = Mode.Tog;
        }

        public enum Mode
        {
            None,
            
            Musou,
            Tog
        }
    }
}