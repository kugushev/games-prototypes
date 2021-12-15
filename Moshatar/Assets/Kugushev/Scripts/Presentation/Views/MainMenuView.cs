using Kugushev.Scripts.Core.Models;
using Kugushev.Scripts.Core.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Presentation.Views
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button fightButton;
        [SerializeField] private Button benchmarkButton;
        [SerializeField] private Button duelButton;
        [SerializeField] private Button battleButton;
        [SerializeField] private Button togButton;

        [Inject] private GameModeService _gameModeService;

        private void Awake()
        {
            fightButton.onClick.AddListener(() => _gameModeService.StartFight());
            benchmarkButton.onClick.AddListener(() => _gameModeService.StartBenchmark());
            duelButton.onClick.AddListener(() => _gameModeService.StartDuel());
            battleButton.onClick.AddListener(() => _gameModeService.StartBattleMusou());
            togButton.onClick.AddListener(() => _gameModeService.StartBattleTog());
        }
    }
}