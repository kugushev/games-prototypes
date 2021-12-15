using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.Services;
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
        [Inject] private BattleGameplayManager _gameplayManager;

        private readonly WaitForSeconds _waitForDamage = new WaitForSeconds(0.5f);

        private void Start()
        {
            foreach (var unit in _enemySquad.Units)
                CreateUnit(unit);

            _enemySquad.Units.ObserveAdd().Subscribe(e => CreateUnit(e.Value)).AddTo(this);

            StartCoroutine(HandleDots());
        }

        private IEnumerator HandleDots()
        {
            // it should be moved to Core
            while (true)
            {
                yield return _waitForDamage;
                foreach (var unit in _enemySquad.Units)
                {
                    if (unit.IsDead)
                        continue;

                    if (unit.Burning)
                        unit.Suffer(_gameplayManager.Parameters.FireBreathDamage);
                }
            }
        }

        private void CreateUnit(EnemyFighter enemyFighter)
        {
            _enemyUnitFactory.Create(enemyFighter.Position.Value.To3D(), enemyFighter);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireCube(Vector3.zero, new Vector3(EnemySquad.SpawnSize * 2f, 1, EnemySquad.SpawnSize * 2f));
        // }
    }
}