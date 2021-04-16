using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.ProceduralGeneration
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(ParliamentGenerator))]
    public class ParliamentGenerator : ScriptableObject
    {
        [SerializeField] private ObjectsPool objectsPool;

        [SerializeField] private PoliticianCharacter[] characters;

        [Header("Rules")] [SerializeField] private float minIncomeProbability = 0.5f;
        [SerializeField] [Range(0.5f, 1f)] private float maxIncomeProbability = 1f;

        [SerializeField] private int minStartBudget;
        [SerializeField] private int maxStartBudget = GameConstants.MaxStartBudget;


        private readonly List<int> _indexesBuffer = new List<int>(9);

        public Parliament Generate(int seed)
        {
            Random.InitState(seed);

            var parliament = objectsPool.GetObject<Parliament, Parliament.State>(new Parliament.State());

            FillIndexesBuffer();
            while (_indexesBuffer.Count > 0)
            {
                var politician = CreatePolitician();

                parliament.AddPolitician(politician);
            }

            _indexesBuffer.Clear();

            return parliament;
        }

        private Politician CreatePolitician()
        {
            int index = NextRandomIndex();
            var character = characters[index];

            var politician = objectsPool.GetObject<Politician, Politician.State>(new Politician.State(character,
                NextIncomeProbability(),
                NextStartBudget(),
                NextTraits()
            ));

            return politician;
        }

        private Percentage NextIncomeProbability() => new Percentage(
            Random.Range(minIncomeProbability, maxIncomeProbability));

        private int NextStartBudget() => Random.Range(minStartBudget, maxStartBudget);

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

        private void FillIndexesBuffer()
        {
            _indexesBuffer.Clear();
            for (int i = 0; i < characters.Length; i++)
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