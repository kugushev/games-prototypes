using System;
using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Presenters
{
    public class EnemyBaseUnitPresenter : BaseUnitPresenter
    {
        [Inject] private SquadController _squadController = default!;
        [Inject] private EnemySquad _enemySquad = default!;

        private readonly EnemyUnit _model = new EnemyUnit();

        protected override BaseUnit Model => _model;

        protected override void OnAwake()
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