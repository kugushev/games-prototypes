using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Presentation.Interfaces;
using Kugushev.Scripts.Common.Presentation.ReactivePresentationModels;
using UniRx;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels.Wayfarers
{
    public class BanditWayfarerRPM : BaseCharacterRPM, IInteractableOwner
    {
        private BanditWayfarer _model = default!;

        [Inject]
        public void Init(BanditWayfarer banditWayfarer)
        {
            _model = banditWayfarer;
        }
        
        IInteractable IInteractableOwner.Interactable => _model;

        protected override ActivityType CurrentActivity => _model.Activity.Value;

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