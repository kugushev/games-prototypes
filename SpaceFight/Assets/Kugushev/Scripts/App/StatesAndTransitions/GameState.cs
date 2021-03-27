using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.App.Utils;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.StatesAndTransitions;

namespace Kugushev.Scripts.App.StatesAndTransitions
{
    internal class GameState : BaseSceneLoadingState<AppModel>
    {
        private readonly GameSceneParametersPipeline _parametersPipeline;

        public GameState(AppModel model, GameSceneParametersPipeline parametersPipeline)
            : base(model, UnityConstants.GameManagementScene, false)
        {
            _parametersPipeline = parametersPipeline;
        }

        protected override void AssertModel()
        {
        }

        protected override void OnEnterBeforeLoadScene()
        {
            var gameInfo = new GameInfo(Model.MainMenu.Seed);
            _parametersPipeline.Set(gameInfo);
        }
    }
}