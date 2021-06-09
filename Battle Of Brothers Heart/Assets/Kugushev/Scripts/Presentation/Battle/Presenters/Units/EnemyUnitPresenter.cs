using System;
using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Common.Exceptions;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Units
{
    public class EnemyUnitPresenter : BaseUnitPresenter
    {
        [Inject] private EnemySquad _enemySquad = default!;

        private EnemyUnit? _model;

        public override BaseUnit Model => _model ?? throw new PropertyIsNotInitializedException(nameof(Model));


        public void Init(EnemyUnit model) => _model = model;
    }
}