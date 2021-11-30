using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Squad
{
    public class EnemySquadPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject enemyUnitPrefab = default!;

        [Inject] private DiContainer _container = default!;
        [Inject] private EnemySquad _enemySquad = default!;

        private readonly List<EnemyUnitPresenter> _presentersBuffer = new List<EnemyUnitPresenter>(1);

        private void Start()
        {
            foreach (var unit in _enemySquad.Units)
            {
                CreateUnit(unit);
            }

            _enemySquad.Units.ObserveAdd().Subscribe(e => CreateUnit(e.Value)).AddTo(this);
        }

        private void CreateUnit(EnemyFighter playerFighter)
        {
            var go = _container.InstantiatePrefab(enemyUnitPrefab, transform);

            _presentersBuffer.Clear();
            go.GetComponents(_presentersBuffer);
            var presenter = _presentersBuffer.Single();

            presenter.Init(playerFighter);

            _presentersBuffer.Clear();
        }
    }
}