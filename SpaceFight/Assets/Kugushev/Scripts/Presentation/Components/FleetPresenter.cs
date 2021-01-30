using System;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components
{
    public class FleetPresenter: BaseComponent<FleetManager>
    {
        [SerializeField] private GameObject armyPrefab;
        protected override void OnAwake()
        {
            
        }

        private void Update()
        {
            if (Model.ArmiesToSent.Count > 0)
            {
                // todo: refactor (pooling, etc.)
                var armyModel = Model.ArmiesToSent.Dequeue();
                
                var army = Instantiate(armyPrefab, transform);
                army.GetComponent<ArmyPresenter>().Army = armyModel;
            }
        }
    }
}