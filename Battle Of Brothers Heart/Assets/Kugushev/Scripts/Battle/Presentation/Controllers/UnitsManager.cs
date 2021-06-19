using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Controllers
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