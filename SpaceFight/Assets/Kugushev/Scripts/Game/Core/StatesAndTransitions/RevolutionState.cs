using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.StatesAndTransitions
{
    internal class RevolutionState : BaseSceneLoadingState<GameModel>
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