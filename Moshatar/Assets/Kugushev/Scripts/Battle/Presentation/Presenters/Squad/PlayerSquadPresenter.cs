using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Squad
{
    public class PlayerSquadPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject playerUnitPrefab;

        [Inject] private readonly HeroUnit _heroUnit;
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly PlayerSquad _playerSquad;

        private readonly List<PlayerUnitPresenter> _presentersBuffer = new List<PlayerUnitPresenter>(1);

        private void Start()
        {
            foreach (var unit in _playerSquad.Units)
                CreateUnit(unit);

            _heroUnit.Init(_playerSquad.Hero);
        }

        private void CreateUnit(PlayerFighter playerFighter)
        {
            var go = _container.InstantiatePrefab(playerUnitPrefab, transform);

            _presentersBuffer.Clear();
            go.GetComponents(_presentersBuffer);
            var presenter = _presentersBuffer.Single();

            presenter.Init(playerFighter);

            _presentersBuffer.Clear();
        }
    }
}