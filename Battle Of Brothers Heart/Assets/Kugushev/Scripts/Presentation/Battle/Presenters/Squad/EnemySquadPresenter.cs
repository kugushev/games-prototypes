using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Presentation.Battle.Presenters.Units;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Squad
{
    public class EnemySquadPresenter : BaseSquadPresenter
    {
        [SerializeField] private GameObject enemyUnitPrefab = default!;

        [Inject] private DiContainer _container = default!;
        [Inject] private EnemySquad _enemySquad = default!;

        private readonly List<EnemyUnitPresenter> _presentersBuffer = new List<EnemyUnitPresenter>(1);

        protected override BaseSquad Squad => _enemySquad;

        private void Start()
        {
            foreach (var unit in _enemySquad.Units)
            {
                CreateUnit(unit);
            }
        }

        private void CreateUnit(EnemyUnit playerUnit)
        {
            var go = _container.InstantiatePrefab(enemyUnitPrefab, transform);

            _presentersBuffer.Clear();
            go.GetComponents(_presentersBuffer);
            var presenter = _presentersBuffer.Single();

            presenter.Init(playerUnit);

            _presentersBuffer.Clear();
        }
    }
}