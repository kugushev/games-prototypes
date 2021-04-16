using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Services
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(PoliticalActionsRepository))]
    public class PoliticalActionsRepository : ScriptableObject
    {
        [SerializeField] private PoliticalAction[]? normal;
        [SerializeField] private PoliticalAction[]? hard;
        [SerializeField] private PoliticalAction[]? insane;
        [SerializeField] private PoliticalAction? stub;

        public PoliticalAction Stub
        {
            get
            {
                Asserting.NotNull(stub);
                return stub;
            }
        }

        public PoliticalAction GetRandom(Difficulty difficulty)
        {
            Asserting.NotNull(normal, hard, insane);

            return difficulty switch
            {
                Difficulty.Normal => GetRandom(normal),
                Difficulty.Hard => GetRandom(hard),
                Difficulty.Insane => GetRandom(insane),
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
            };
        }

        private PoliticalAction GetRandom(PoliticalAction[] actions)
        {
            int index = Random.Range(0, actions.Length);
            return actions[index];
        }
    }
}