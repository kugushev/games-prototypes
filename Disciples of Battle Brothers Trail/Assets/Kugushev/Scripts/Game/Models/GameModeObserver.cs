using System;
using Kugushev.Scripts.Common.Enums;
using Kugushev.Scripts.Common.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    public class GameModeObserver : MonoBehaviour
    {
        [SerializeField] private GameObject gameRoot;

        private void Update()
        {
            switch (GameModeManager.Instance.Current)
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