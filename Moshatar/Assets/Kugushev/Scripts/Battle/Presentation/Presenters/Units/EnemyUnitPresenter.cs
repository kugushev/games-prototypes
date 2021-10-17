using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter
    {
        [Inject] private EnemySquad _enemySquad = default!;

        private EnemyFighter? _model;

        public override BaseFighter Model => _model ?? throw new PropertyIsNotInitializedException();


        public void Init(EnemyFighter model) => _model = model;
    }
}