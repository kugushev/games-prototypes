using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.ValueObjects;
using Kugushev.Scripts.Campaign.MissionSelection.Interfaces;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.ContextManagement.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.Campaign.MissionSelection.PresentationModels
{
    public class MissionSelectionPresentationModel : MonoBehaviour, IMissionSelectionPresentationModel
    {
        [SerializeField] private Button finishCampaignButton = default!;
        [SerializeField] private Button startMissionButton = default!;
        [SerializeField] private ToggleGroup missionsToggleGroup = default!;
        [SerializeField] private TextMeshProUGUI budgetText = default!;

        [Inject] private ICampaignMissions _model = default!;
        [Inject] private MissionCardPresentationModel.Factory _missionCardsFactory = default!;
        [Inject] private SignalBus _signalBus = default!;
        [Inject] private SignalToTransition<MissionParameters>.Factory _startMissionSignalFactory = default!;
        [Inject] private SignalToTransition<CampaignExitParameters>.Factory _finishCampaignSignalFactory = default!;

        private readonly Dictionary<MissionInfo, MissionCardPresentationModel> _missionCards =
            new Dictionary<MissionInfo, MissionCardPresentationModel>(16);

        private readonly ReactiveProperty<MissionInfo?> _selectedMission = new ReactiveProperty<MissionInfo?>();

        private void Start()
        {
            foreach (var mission in _model.Missions)
                AddMissionCard(mission);

            _model.Missions.ObserveAdd().Subscribe(e => AddMissionCard(e.Value));
            _model.Missions.ObserveRemove().Subscribe(e => RemoveIntrigueCard(e.Value));

            _model.Budget.Select(StringBag.FromInt).SubscribeToTextMeshPro(budgetText);

            _selectedMission.Select(v => v is { }).SubscribeToInteractable(startMissionButton);
            startMissionButton.onClick.AddListener(StartMission);
            finishCampaignButton.onClick.AddListener(FinishCampaign);
        }

        ToggleGroup IMissionSelectionPresentationModel.ToggleGroup => missionsToggleGroup;

        void IMissionSelectionPresentationModel.SelectCard(MissionInfo? card) => _selectedMission.Value = card;

        private void AddMissionCard(MissionInfo missionInfo)
        {
            if (_missionCards.ContainsKey(missionInfo))
            {
                Debug.LogError($"Mission {missionInfo} is already set");
                return;
            }

            var card = _missionCardsFactory.Create(missionInfo, this);
            _missionCards.Add(missionInfo, card);
        }

        private void RemoveIntrigueCard(MissionInfo missionInfo)
        {
            if (_selectedMission.Value == missionInfo)
                _selectedMission.Value = null;

            if (_missionCards.TryGetValue(missionInfo, out var card))
            {
                card.Dispose();
                _missionCards.Remove(missionInfo);
            }
            else
                Debug.LogError($"Intrigue {missionInfo} is not set");
        }

        private void StartMission()
        {
            if (_selectedMission.Value != null)
            {
                var signal = _startMissionSignalFactory.Create(new MissionParameters(_selectedMission.Value));
                _signalBus.Fire(signal);
            }
            else
                Debug.LogError("Selected mission is null");
        }

        private void FinishCampaign() => _signalBus.Fire(_finishCampaignSignalFactory.Create(default));
    }
}