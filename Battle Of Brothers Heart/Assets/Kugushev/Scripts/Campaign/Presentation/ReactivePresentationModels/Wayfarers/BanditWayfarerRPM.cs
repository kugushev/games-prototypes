using System;
using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Presentation.Interfaces;
using Kugushev.Scripts.Common.Presentation.ReactivePresentationModels;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels.Wayfarers
{
    public class BanditWayfarerRPM : BaseCharacterRPM, IInteractableOwner
    {
        [SerializeField] private TextMeshProUGUI powerText = default!;

        private BanditWayfarer _model = default!;

        [Inject]
        public void Init(BanditWayfarer banditWayfarer)
        {
            _model = banditWayfarer;
        }

        public BanditWayfarer Model => _model;

        IInteractable IInteractableOwner.Interactable => _model;

        protected override ActivityType CurrentActivity => _model.Activity.Value;

        private void Awake()
        {
            powerText.text = _model.BanditWorldUnit.Characters.Count.ToString();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _model.Position.Subscribe(OnPositionChanged).AddTo(this);
            _model.Direction.Subscribe(OnDirectionChanged).AddTo(this);
            _model.Activity.Subscribe(OnActivityChanged).AddTo(this);
        }

        public class Factory : PlaceholderFactory<BanditWayfarer, BanditWayfarerRPM>
        {
        }
    }
}