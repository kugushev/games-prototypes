using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Common.Core.Exceptions;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter
    {
        [Inject] private EnemySquad _enemySquad = default!;

        private EnemyUnit? _model;

        public override BaseUnit Model => _model ?? throw new PropertyIsNotInitializedException();


        public void Init(EnemyUnit model) => _model = model;
    }
}