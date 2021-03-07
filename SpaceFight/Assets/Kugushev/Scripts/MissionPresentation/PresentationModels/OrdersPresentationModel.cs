using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Mission.Player;
using Kugushev.Scripts.MissionPresentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.PresentationModels
{
    public class OrdersPresentationModel: BasePresentationModel
    {
        [SerializeField] private OrdersManager model;
        protected override IModel Model => model;
    }
}