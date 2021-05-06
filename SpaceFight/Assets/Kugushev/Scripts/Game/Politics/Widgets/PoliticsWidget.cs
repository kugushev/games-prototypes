﻿using Kugushev.Scripts.Game.Politics.PresentationModels;
using UnityEngine;

namespace Kugushev.Scripts.Game.Politics.Widgets
{
    public class PoliticsWidget : MonoBehaviour
    {
        // [SerializeField] private GameModelProvider? gameModelProvider;

        [SerializeField] private ParliamentPresentationModel? parliament;
        [SerializeField] private IntriguesPresentationModel? politicalActions;
        [SerializeField] private CampaignPreparationWidget? campaignPreparation;
        [SerializeField] private RevolutionWidget? revolutionWidget;

        // private void Start()
        // {
        //     Asserting.NotNull(gameModelProvider, parliament, politicalActions, campaignPreparation, revolutionWidget);
        //
        //     if (gameModelProvider.TryGetModel(out var model))
        //     {
        //         parliament.Setup(model.Parliament);
        //         politicalActions.Setup(model, model.Parliament);
        //         campaignPreparation.SetUp(model.CampaignPreparation);
        //         revolutionWidget.SetUp(model.Parliament);
        //     }
        //     else
        //         Debug.LogError("Unable to get model");
        // }
    }
}