using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class MissionDebriefingWidget : MonoBehaviour
    {
        [SerializeField] private MissionModelProvider missionModelProvider;
        [SerializeField] private TextMeshProUGUI youWinText;
        [SerializeField] private TextMeshProUGUI aiWinText;
        [SerializeField] private Transform achievementsPanel;
        [SerializeField] private GameObject achievementCardPrefab;
        [SerializeField] private ToggleGroup toggleGroup;

        private void Start()
        {
            if (missionModelProvider.TryGetModel(out var missionModel))
            {
                if (missionModel.ExecutionResult == null)
                {
                    Debug.LogError("No Execution Result found");
                    return;
                }

                switch (missionModel.ExecutionResult.Value.Winner)
                {
                    case Faction.Green:
                        aiWinText.enabled = false;
                        break;
                    case Faction.Red:
                        youWinText.enabled = false;
                        break;
                    default:
                        Debug.LogError($"Unexpected winner {missionModel.ExecutionResult.Value.Winner}");
                        break;
                }

                foreach (var achievement in missionModel.DebriefingSummary.AllAchievements)
                {
                    var go = Instantiate(achievementCardPrefab, achievementsPanel);
                    var widget = go.GetComponent<AchievementCardWidget>();
                    widget.SetUp(achievement.Info, missionModel.DebriefingSummary, toggleGroup);
                }
            }
        }
    }
}