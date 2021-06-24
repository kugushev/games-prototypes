using Kugushev.Scripts.City.Core.Models.Villagers;
using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Presentation.ReactivePresentationModels;
using UniRx;
using Zenject;

namespace Kugushev.Scripts.City.Presentation.ReactivePresentationModels.Villagers
{
    public class PlayerVillagerRPM : BaseCharacterRPM
    {
        [Inject] private PlayerVillager _model = default!;

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