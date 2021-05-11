using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Services;
using Zenject;

namespace Kugushev.Scripts.Game.Core
{
    public class GameDataInitializer : IInitializable
    {
        private readonly ParametersPipeline<GameParameters> _parametersPipeline;
        private readonly ParliamentGenerationService _parliamentGenerationService;

        private readonly Parliament _parliament;

        internal GameDataInitializer(ParametersPipeline<GameParameters> parametersPipeline,
            ParliamentGenerationService parliamentGenerationService, Parliament parliament)
        {
            _parametersPipeline = parametersPipeline;
            _parliamentGenerationService = parliamentGenerationService;
            _parliament = parliament;
        }

        public bool Initialized { get; private set; }

        void IInitializable.Initialize()
        {
            var parameters = _parametersPipeline.Pop();

            var politicians = _parliamentGenerationService.GeneratePolitician(parameters.Seed);
            _parliament.Init(politicians);

            Initialized = true;
        }
    }
}