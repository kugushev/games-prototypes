using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Squad
{
    public class PlayerSquadPresenter : BaseSquadPresenter
    {
        [SerializeField] private GameObject playerUnitPrefab = default!;

        [Inject] private DiContainer _container = default!;
        [Inject] private PlayerSquad _playerSquad = default!;

        private readonly List<PlayerUnitPresenter> _presentersBuffer = new List<PlayerUnitPresenter>(1);

        protected override BaseSquad Squad => _playerSquad;

        private void Start()
        {
            foreach (var unit in _playerSquad.Units)
            {
                CreateUnit(unit);
            }
        }

        private void CreateUnit(PlayerUnit playerUnit)
        {
            var go = _container.InstantiatePrefab(playerUnitPrefab, transform);

            _presentersBuffer.Clear();
            go.GetComponents(_presentersBuffer);
            var presenter = _presentersBuffer.Single();

            presenter.Init(playerUnit);

            _presentersBuffer.Clear();
        }
    }
}