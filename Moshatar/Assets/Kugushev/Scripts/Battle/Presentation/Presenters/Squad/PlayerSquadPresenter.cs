using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Core.Services;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Squad
{
    public class PlayerSquadPresenter : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private TextMeshProUGUI unitsLeft;
        [SerializeField] private DefendingPointPresenter defendingPointPrefab;

        [Inject] private readonly HeroUnit _heroUnit;
        [Inject] private readonly PlayerSquad _playerSquad;
        [Inject] private readonly PlayerUnitPresenter.Factory _playerUnitFactory;
        [Inject] private readonly GameModeService _gameModeService;

        private void Start()
        {
            _playerSquad.SpawnPoints = spawnPoints.Select(t => t.position).ToArray();

            _playerSquad.AvailableUnits.Select(StringBag.FromInt).Subscribe(s => unitsLeft.text = s).AddTo(this);

            foreach (var unit in _playerSquad.Units)
                CreateUnit(unit);

            _playerSquad.Units.ObserveAdd().Subscribe(e => CreateUnit(e.Value)).AddTo(this);

            _heroUnit.Init(_playerSquad.Hero);

            foreach (var defendingPoint in _playerSquad.DefendingPoints)
            {
                var presenter = Instantiate(defendingPointPrefab,
                    defendingPoint.Position.Value.To3D(),
                    Quaternion.identity,
                    transform);

                presenter.Init(defendingPoint);
            }

            _playerSquad.GameOver += () => _gameModeService.BackToMenu();
        }

        private void CreateUnit(PlayerFighter playerFighter)
        {
            _playerUnitFactory.Create(playerFighter.Position.Value.To3D(), playerFighter);
        }
    }
}