using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class MissionDebriefingWidget : MonoBehaviour
    {
        [SerializeField] private MissionModelProvider? missionModelProvider;
        [SerializeField] private TextMeshProUGUI? youWinText;
        [SerializeField] private TextMeshProUGUI? aiWinText;
        [SerializeField] private TextMeshProUGUI? tipText;
        [SerializeField] private Transform? achievementsPanel;
        [SerializeField] private GameObject? achievementCardPrefab;
        [SerializeField] private ToggleGroup? toggleGroup;

        private void Start()
        {
            Asserting.NotNull(missionModelProvider, youWinText, aiWinText, tipText, achievementsPanel,
                achievementCardPrefab, toggleGroup);

            if (missionModelProvider.TryGetModel(out var missionModel))
            {
                if (missionModel.ExecutionResult == null)
                {
                    Debug.LogError("No Execution Result found");
                    return;
                }

                if (missionModel.ExecutionResult.Value.Winner == missionModel.PlayerFaction)
                {
                    aiWinText.enabled = false;
                    tipText.enabled = true;
                }
                else if (missionModel.ExecutionResult.Value.Winner == missionModel.PlayerFaction.GetOpposite())
                {
                    youWinText.enabled = false;
                    tipText.enabled = false;
                }
                else
                    Debug.LogError($"Unexpected winner {missionModel.ExecutionResult.Value.Winner}");

                if (missionModel.DebriefingSummary != null)
                    foreach (var perk in missionModel.DebriefingSummary.AllPerks)
                    {
                        var go = Instantiate(achievementCardPrefab, achievementsPanel);
                        var widget = go.GetComponent<PerkCardWidget>();
                        widget.SetUp(perk.Info, missionModel.DebriefingSummary, toggleGroup);
                    }
                else
                    Debug.LogError("Debriefing summary is null");
            }
        }
    }
}