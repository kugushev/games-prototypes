using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter
    {
        [Inject] private SquadController _squadController = default!;
        [Inject] private EnemySquad _enemySquad = default!;

        private readonly EnemyUnit _model = new EnemyUnit();

        public override BaseUnit Model => _model;

        protected override void OnStart()
        {
            _enemySquad.Add(_model);
        }

        public void Clicked()
        {
            if (Input.GetMouseButton(1))
                _squadController.EnemyUnitClicked(_model);
        }
    }
}