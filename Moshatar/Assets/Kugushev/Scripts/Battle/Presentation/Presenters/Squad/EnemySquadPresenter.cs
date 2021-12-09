﻿using System;
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
        [Inject] private EnemySquad _enemySquad = default!;
        [Inject] private EnemyUnitPresenter.Factory _enemyUnitFactory;

        private void Start()
        {
            foreach (var unit in _enemySquad.Units) 
                CreateUnit(unit);

            _enemySquad.Units.ObserveAdd().Subscribe(e => CreateUnit(e.Value)).AddTo(this);
        }

        private void CreateUnit(EnemyFighter playerFighter)
        {
            _enemyUnitFactory.Create(playerFighter.Position.Value.To3D(), playerFighter);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(EnemySquad.SpawnSize * 2f, 1, EnemySquad.SpawnSize * 2f));
        }
    }
}