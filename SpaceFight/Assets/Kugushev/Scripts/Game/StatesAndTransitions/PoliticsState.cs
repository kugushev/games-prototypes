using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    public class PoliticsState : BaseSceneLoadingState<GameModel>
    {
        public PoliticsState(GameModel model) 
            : base(model, UnityConstants.PoliticsMenuScene, true)
        {
        }

        protected override void AssertModel()
        {
            
        }
    }
}