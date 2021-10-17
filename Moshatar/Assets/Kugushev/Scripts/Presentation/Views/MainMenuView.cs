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
        [SerializeField] private TextMeshProUGUI scoreText;

        [Inject] private GameModeService _gameModeService;
        [Inject] private Score _score;

        private void Awake()
        {
            fightButton.onClick.AddListener(() => _gameModeService.StartFight());
            benchmarkButton.onClick.AddListener(() => _gameModeService.StartBenchmark());
            duelButton.onClick.AddListener(() => _gameModeService.StartDuel());
            battleButton.onClick.AddListener(() => _gameModeService.StartBattle());

            scoreText.text = $"Last {_score.LastGold}. Top {_score.TopGold}";
        }
    }
}