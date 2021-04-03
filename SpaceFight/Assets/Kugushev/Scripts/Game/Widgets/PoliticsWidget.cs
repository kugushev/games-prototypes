using Kugushev.Scripts.Game.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticsWidget : MonoBehaviour
    {
        [SerializeField] private GameModelProvider gameModelProvider;

        [SerializeField] private ParliamentWidget parliament;
        [SerializeField] private PoliticalActionsWidget politicalActions;
        [SerializeField] private CampaignPreparationWidget campaignPreparation;
        [SerializeField] public RevolutionWidget revolutionWidget;

        private void Start()
        {
            if (gameModelProvider.TryGetModel(out var model))
            {
                parliament.Setup(model.Parliament);
                politicalActions.Setup(model, model.Parliament);
                campaignPreparation.SetUp(model.CampaignPreparation);
                revolutionWidget.SetUp(model.Parliament);
            }
            else
                Debug.LogError("Unable to get model");
        }
    }
}