using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Campaign.Widgets
{
    public class MissionSelectionWidget : MonoBehaviour
    {
        [SerializeField] private CampaignModelProvider modelProvider;

        [SerializeField] private Transform missionsPanel;
        [SerializeField] private GameObject missionCardPrefab;
        [SerializeField] private ToggleGroup missionsToggleGroup;

        private void Start()
        {
            if (TryGetModel(out var model))
            {
                SetupMissions(model);
            }
        }

        private void SetupMissions(MissionSelection model)
        {
            foreach (var mission in model.Missions)
            {
                var go = Instantiate(missionCardPrefab, missionsPanel);
                var widget = go.GetComponent<MissionCardWidget>();
                widget.SetUp(mission, model, missionsToggleGroup);
            }
        }

        private bool TryGetModel(out MissionSelection model)
        {
            model = null;
            if (modelProvider.TryGetModel(out var rootModel))
            {
                if (rootModel.MissionSelection != null)
                {
                    model = rootModel.MissionSelection;
                    return true;
                }

                Debug.LogError($"{nameof(rootModel.MissionSelection)} is null");
            }

            return false;
        }
    }
}