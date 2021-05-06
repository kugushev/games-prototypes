using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.StatesAndTransitions
{
    internal class PoliticsState : BaseSceneLoadingState<GameModel>
    {
        public PoliticsState(GameModel model)
            : base(model, UnityConstants.PoliticsMenuScene, true)
        {
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            Model.PrepareNextRound();
        }
    }
}