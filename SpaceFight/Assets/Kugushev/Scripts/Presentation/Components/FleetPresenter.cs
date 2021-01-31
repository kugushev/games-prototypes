using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components
{
    public class FleetPresenter : BaseComponent<FleetManager>
    {
        [SerializeField] private GameObject armyPrefab;

        private readonly Queue<ArmyPresenter> _armiesPool =
            new Queue<ArmyPresenter>(GameConstants.ArmiesPerFleetCapacity);

        protected override void OnAwake()
        {
        }

        private void Update()
        {
            SendArmyIfRequired();
        }

        private void SendArmyIfRequired()
        {
            if (Model.ArmiesToSent.Count > 0)
            {
                var army = Model.ArmiesToSent.Dequeue();

                var presenter = GetArmy();
                presenter.Army = army;
                presenter.Send();
            }
        }

        private ArmyPresenter GetArmy()
        {
            if (_armiesPool.Count > 0)
            {
                var fromPool = _armiesPool.Dequeue();
                fromPool.gameObject.SetActive(true);
                return fromPool;
            }
            
            var go = Instantiate(armyPrefab, transform);
            var armyPresenter = go.GetComponent<ArmyPresenter>();
            armyPresenter.SetOwner(this);
            return armyPresenter;
        }

        public void ReturnArmyToPool(ArmyPresenter armyPresenter)
        {
            armyPresenter.Army.Dispose();
            armyPresenter.Army = null;
            armyPresenter.gameObject.SetActive(false);
            _armiesPool.Enqueue(armyPresenter);
        }
    }
}