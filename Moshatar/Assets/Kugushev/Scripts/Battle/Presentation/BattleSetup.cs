using System;
using Kugushev.Scripts.Battle.Core.Services;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation
{
    public class BattleSetup: MonoBehaviour
    {
        [SerializeField] private GameObject musouEnv;
        [SerializeField] private GameObject togEnv;

        [Inject] private BattleGameplayManager _battleGameplayManager;
        
        private void Start()
        {
            SetupEnvironment();
        }

        private void SetupEnvironment()
        {
            switch (_battleGameplayManager.SeletedMode)
            {
                case BattleGameplayManager.Mode.None:
                    Debug.LogError("No mode specified");
                    break;
                case BattleGameplayManager.Mode.Musou:
                    Instantiate(musouEnv, transform);
                    break;
                case BattleGameplayManager.Mode.Tog:
                    Instantiate(togEnv, transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}