using Kugushev.Scripts.Battle.Core.Models.Squad;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Squad
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