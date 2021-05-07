using Kugushev.Scripts.App.Core.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Services;
using Zenject;

namespace Kugushev.Scripts.Game.Core
{
    public class GameDataStore : IInitializable
    {
        private readonly ParametersPipeline<GameParameters> _parametersPipeline;
        private readonly ParliamentGenerationService _parliamentGenerationService;

        private Parliament? _parliament;
        private readonly Intrigues _intrigues = new Intrigues();

        public GameDataStore(ParametersPipeline<GameParameters> parametersPipeline,
            ParliamentGenerationService parliamentGenerationService)
        {
            _parametersPipeline = parametersPipeline;
            _parliamentGenerationService = parliamentGenerationService;
        }

        public bool Initialized { get; private set; }

        public Parliament Parliament => _parliament ?? throw new SpaceFightException("Store is not initialized");

        public IIntrigues Intrigues => _intrigues;

        void IInitializable.Initialize()
        {
            // todo: run it in a separate thread

            var parameters = _parametersPipeline.Pop();
            _parliament = _parliamentGenerationService.Generate(parameters.Seed);

            Initialized = true;
        }
    }
}