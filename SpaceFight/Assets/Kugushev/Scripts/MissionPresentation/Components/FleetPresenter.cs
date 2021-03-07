using System.Collections.Generic;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.Components.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.Components
{
    public class FleetPresenter : BaseComponent<Fleet>
    {
        [SerializeField] private Transform limbo;
        [SerializeField] private GameObject armyPrefab;

        private readonly Queue<ArmyPresenter> _armiesPool =
            new Queue<ArmyPresenter>(GameplayConstants.ArmiesPerFleetCapacity);

        protected override void OnAwake()
        {
        }

        protected override void OnStart()
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
                presenter.SendFollowingOrder();
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
            armyPresenter.transform.position = limbo.position;
            _armiesPool.Enqueue(armyPresenter);
        }
    }
}