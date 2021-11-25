using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class PlayerUnitPresenter : BaseUnitPresenter
    {
        private PlayerFighter _model;
        public override BaseFighter Model => _model ?? throw new PropertyIsNotInitializedException();

        public void Init(PlayerFighter model) => _model = model;

        protected override void OnActivityChanged(ActivityType newActivityType)
        {
            
        }

        protected override void OnAttacking()
        {
            
        }

        protected override void OnHurt(BaseFighter attacker)
        {
        }

        protected override void OnDie()
        {
        }
    }
}