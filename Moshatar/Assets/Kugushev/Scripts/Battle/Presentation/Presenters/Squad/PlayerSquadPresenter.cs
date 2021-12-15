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
    public class PlayerSquadPresenter : MonoBehaviour
    {
        [Inject] private readonly HeroUnit _heroUnit;
        [Inject] private readonly PlayerSquad _playerSquad;
        [Inject] private readonly PlayerUnitPresenter.Factory _playerUnitFactory;

        private void Start()
        {
            foreach (var unit in _playerSquad.Units)
                CreateUnit(unit);

            _playerSquad.Units.ObserveAdd().Subscribe(e => CreateUnit(e.Value)).AddTo(this);

            _heroUnit.Init(_playerSquad.Hero);
        }

        private void CreateUnit(PlayerFighter playerFighter)
        {
            _playerUnitFactory.Create(playerFighter.Position.Value.To3D(), playerFighter);
        }
    }
}