using System;
using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Controllers
{
    public class UnitsManager: MonoBehaviour
    {
        [Inject] private EnemySquad _enemySquad = default!;

        private void FixedUpdate()
        {
            _enemySquad.ProcessOrders(new DeltaTime(Time.fixedDeltaTime));
        }
    }
}