using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Presentation.ReactivePresentationModels;
using UniRx;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels.Wayfarers
{
    public class PlayerWayfarerRPM : BaseCharacterRPM
    {
        private PlayerWayfarer _model = default!;

        [Inject]
        public void Init(WayfarersManager manager)
        {
            _model = manager.Player;
        }

        protected override ActivityType CurrentActivity => _model.Activity.Value;

        protected override void OnStart()
        {
            base.OnStart();

            _model.Position.Subscribe(OnPositionChanged).AddTo(this);
            _model.Direction.Subscribe(OnDirectionChanged).AddTo(this);
            _model.Activity.Subscribe(OnActivityChanged).AddTo(this);
        }
    }
}