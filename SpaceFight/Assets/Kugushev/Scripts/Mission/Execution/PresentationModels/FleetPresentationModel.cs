using System;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Execution.Interfaces;
using Kugushev.Scripts.MissionPresentation.Components;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Mission.Execution.PresentationModels
{
    public class FleetPresentationModel : MonoBehaviour, IFleetPresentationModel
    {
        [SerializeField] private Faction faction;
        [SerializeField] private Transform limbo = default!;

        private Fleet _model = default!;

        [Inject] private ArmyPresentationModel.Factory _armiesFactory = default!;

        [Inject]
        public void Init(GreenFleet greenFleet, RedFleet redFleet)
        {
            _model = faction switch
            {
                Faction.Green => greenFleet,
                Faction.Red => redFleet,
                _ => throw new ArgumentOutOfRangeException(nameof(faction), faction, "Invalid fleet faction")
            };

            _model.OrderCommitted += SendArmyIfRequired;
        }

        private void OnDestroy() => _model.OrderCommitted -= SendArmyIfRequired;

        public Vector3 AssemblyPosition => limbo.position;

        private void SendArmyIfRequired()
        {
            while (_model.TryExtractArmy(out var army))
            {
                var presenter = _armiesFactory.Create(army, this);
                presenter.Army = army;
                presenter.SendFollowingOrder();
            }
        }
    }
}