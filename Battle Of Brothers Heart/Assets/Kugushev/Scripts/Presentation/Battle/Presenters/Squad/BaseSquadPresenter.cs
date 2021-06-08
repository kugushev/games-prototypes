using Kugushev.Scripts.Core.Battle.Models.Squad;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Presentation.Battle.Presenters.Units;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Squad
{
    public abstract class BaseSquadPresenter : MonoBehaviour
    {
        protected abstract BaseSquad Squad { get; }
        
        private void FixedUpdate()
        {
            Squad.ProcessOrders(new DeltaTime(Time.fixedDeltaTime));
        }
    }
}