using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Utils;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class MissionDebriefingWidget: MonoBehaviour
    {
        [SerializeField] private MissionModelProvider missionModelProvider;
        [SerializeField] private TextMeshProUGUI youWinText;
        [SerializeField] private TextMeshProUGUI aiWinText;

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
            }
        }
    }
}