using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Core.Repositories
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(IntriguesRepository))]
    public class IntriguesRepository : ScriptableObject
    {
        [SerializeField] private Intrigue[]? normal;
        [SerializeField] private Intrigue[]? hard;
        [SerializeField] private Intrigue[]? insane;
        [SerializeField] private Intrigue? stub;

        public Intrigue Stub
        {
            get
            {
                Asserting.NotNull(stub);
                return stub;
            }
        }

        public Intrigue GetRandom(Difficulty difficulty)
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

        private Intrigue GetRandom(Intrigue[] actions)
        {
            int index = Random.Range(0, actions.Length);
            return actions[index];
        }
    }
}