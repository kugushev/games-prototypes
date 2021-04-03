using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    public class RevolutionState : BaseSceneLoadingState<GameModel>
    {
        public RevolutionState(GameModel model)
            : base(model, UnityConstants.RevolutionScene, true)
        {
        }

        protected override void AssertModel()
        {
        }
    }
}