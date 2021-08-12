using System;
using Kugushev.Scripts.Common.Enums;
using Kugushev.Scripts.Game.Managers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Models
{
    public class GameModeObserver : MonoBehaviour
    {
        [SerializeField] private GameObject gameRoot;
        [Inject] private GameModeManager _gameModeManager;

        private void Update()
        {
            switch (_gameModeManager.Current)
            {
                case GameMode.None:
                    break;
                case GameMode.Game:
                    if (!gameRoot.activeSelf)
                        gameRoot.SetActive(true);
                    break;
                case GameMode.City:
                case GameMode.Battle:
                    if (gameRoot.activeSelf)
                        gameRoot.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}