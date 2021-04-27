﻿using System.Collections.Generic;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Game.ValueObjects;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameDateStore : IInitializable
    {
        private readonly ParametersPipeline<GameParameters> _parametersPipeline;
        private readonly ParliamentGenerationService _parliamentGenerationService;

        private readonly List<IntrigueRecord> _politicianCards = new List<IntrigueRecord>(64);
        private Parliament? _parliament;

        public GameDateStore(ParametersPipeline<GameParameters> parametersPipeline,
            ParliamentGenerationService parliamentGenerationService)
        {
            _parametersPipeline = parametersPipeline;
            _parliamentGenerationService = parliamentGenerationService;
        }

        public bool Initialized { get; private set; }

        public Parliament Parliament => _parliament ?? throw new SpaceFightException("Store is not initialized");

        public IReadOnlyList<IntrigueRecord> PoliticianCards => _politicianCards;

        public void Initialize()
        {
            // todo: run it in a separate thread

            var parameters = _parametersPipeline.Pop();
            _parliament = _parliamentGenerationService.Generate(parameters.Seed);

            Initialized = true;
        }
    }
}