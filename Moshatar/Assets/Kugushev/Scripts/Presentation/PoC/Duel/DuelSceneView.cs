using System;
using Kugushev.Scripts.Core.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class DuelSceneView : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        
        [Inject] private GameModeService _gameModeService;
        
        private void Awake()
        {
            backButton.onClick.AddListener(() => _gameModeService.BackToMenu());
        }
    }
}