using Kugushev.Scripts.Common.Entities.Abstractions;
using Kugushev.Scripts.Mission.Player;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class OrdersPresentationModel: BasePresentationModel
    {
        [SerializeField] private OrdersManager model;
        protected override IModel Model => model;
    }
}