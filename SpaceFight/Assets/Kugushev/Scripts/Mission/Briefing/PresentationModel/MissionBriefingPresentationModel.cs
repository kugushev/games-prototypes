﻿using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Mission.Briefing.PresentationModel
{
    public class MissionBriefingPresentationModel : MonoBehaviour
    {
        [SerializeField] private Slider slider = default!;
        [SerializeField] private Button startMissionButton = default!;

        [Inject] private IPlanetarySystem _planetarySystem = default!;

        private void Start()
        {
            slider.onValueChanged.AddListener(AdjustTime);
            startMissionButton.onClick.AddListener(StartMission);
        }

        private void AdjustTime(float sliderValue)
        {
            var dayOfYear = Mathf.FloorToInt(GameplayConstants.DaysInYear * sliderValue);
            _planetarySystem.SetDayOfYear(dayOfYear);
        }

        private void StartMission()
        {
            // todo: fire signal or trigger transition directly
        }
    }
}