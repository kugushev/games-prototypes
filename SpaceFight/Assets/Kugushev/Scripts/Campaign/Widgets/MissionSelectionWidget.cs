using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class MissionSelectionWidget : MonoBehaviour
    {
        [SerializeField] private CampaignModelProvider? modelProvider;

        [SerializeField] private Transform? missionsPanel;
        [SerializeField] private GameObject? missionCardPrefab;
        [SerializeField] private ToggleGroup? missionsToggleGroup;
        [SerializeField] private Button? startMissionButton;
        [SerializeField] private TextMeshProUGUI? budgetText;

        private void Start()
        {
            Asserting.NotNull(budgetText);

            if (TryGetModel(out var model))
            {
                budgetText.text = StringBag.FromInt(model.Budget);
                SetupMissions(model);
            }
        }

        private void SetupMissions(MissionSelection model)
        {
            Asserting.NotNull(missionCardPrefab, missionsPanel, missionsToggleGroup);

            foreach (var mission in model.Missions)
            {
                var go = Instantiate(missionCardPrefab, missionsPanel);
                var widget = go.GetComponent<MissionCardWidget>();
                widget.SetUp(mission, model, missionsToggleGroup);
            }
        }

        private bool TryGetModel([NotNullWhen(true)] out MissionSelection? model)
        {
            Asserting.NotNull(modelProvider);

            model = null;
            if (modelProvider.TryGetModel(out var rootModel))
            {
                model = rootModel.MissionSelection;
                return true;
            }

            return false;
        }

        private void Update()
        {
            Asserting.NotNull(startMissionButton);

            if (TryGetModel(out var model))
                startMissionButton.interactable = model.SelectedMission != null && model.Budget > 0;
        }
    }
}