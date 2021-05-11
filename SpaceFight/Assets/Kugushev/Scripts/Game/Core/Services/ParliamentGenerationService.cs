using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Repositories;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Core.Services
{
    public class ParliamentGenerationService
    {
        private readonly PoliticianCharactersRepository _charactersRepository;

        public ParliamentGenerationService(PoliticianCharactersRepository charactersRepository)
        {
            _charactersRepository = charactersRepository;
        }

        private readonly List<int> _indexesBuffer = new List<int>(GameConstants.ParliamentSize);

        public IReadOnlyList<IPolitician> GeneratePolitician(int seed)
        {
            Random.InitState(seed);

            var politicians = new List<Politician>(GameConstants.ParliamentSize);

            var characters = _charactersRepository.Characters;
            Assert.AreEqual(characters.Count, GameConstants.ParliamentSize);

            FillIndexesBuffer(characters);

            while (_indexesBuffer.Count > 0)
            {
                var politician = CreatePolitician(characters);

                politicians.Add(politician);
            }

            _indexesBuffer.Clear();

            return politicians;
        }

        private Politician CreatePolitician(IReadOnlyList<PoliticianCharacter> characters)
        {
            int index = NextRandomIndex();
            var character = characters[index];

            return new Politician(character,
                NextIncomeProbability(),
                NextStartBudget(),
                NextTraits()
            );
        }

        private Percentage NextIncomeProbability() => new Percentage(
            Random.Range(GameConstants.MinIncomeProbability, GameConstants.MaxIncomeProbability));

        private int NextStartBudget() => Random.Range(GameConstants.MinStartBudget, GameConstants.MaxStartBudget);

        private Traits NextTraits()
        {
            return new Traits(
                NextTrait(),
                NextTrait(),
                NextTrait(),
                NextTrait(),
                NextTrait()
            );

            int NextTrait() => Random.Range(GameConstants.MinTraitValue, GameConstants.MaxTraitValue + 1);
        }

        private void FillIndexesBuffer(IReadOnlyList<PoliticianCharacter> characters)
        {
            _indexesBuffer.Clear();
            for (int i = 0; i < characters.Count; i++)
                _indexesBuffer.Add(i);
        }

        private int NextRandomIndex()
        {
            if (_indexesBuffer.Count == 0)
                throw new Exception("Indexes buffer is empty");

            var bufferIndex = Random.Range(0, _indexesBuffer.Count);
            var index = _indexesBuffer[bufferIndex];
            _indexesBuffer.RemoveAt(bufferIndex);
            return index;
        }
    }
}