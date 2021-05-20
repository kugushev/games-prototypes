using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Enums;
using UnityEngine;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.MissionPresentation.Components
{
    public class FleetPresentationModel : MonoBehaviour
    {
        [SerializeField] private Faction faction;
        [SerializeField] private Transform limbo = default!;
        [SerializeField] private GameObject armyPrefab = default!;

        private Fleet _model = default!;
        private readonly List<ArmyPresentationModel> _armiesBuffer = new List<ArmyPresentationModel>(1);

        [Inject]
        public void Init(GreenFleet greenFleet, RedFleet redFleet)
        {
            _model = faction switch
            {
                Faction.Green => greenFleet,
                Faction.Red => redFleet,
                _ => throw new ArgumentOutOfRangeException(nameof(faction), faction, "Invalid fleet faction")
            };

            _model.OrderCommitted += SendArmyIfRequired;
        }

        private void OnDestroy() => _model.OrderCommitted -= SendArmyIfRequired;

        private readonly Queue<ArmyPresentationModel> _armiesPool =
            new Queue<ArmyPresentationModel>(GameplayConstants.ArmiesPerFleetCapacity);


        private void SendArmyIfRequired()
        {
            while (_model.TryExtractArmy(out var army))
            {
                var presenter = GetArmy();
                presenter.Army = army;
                presenter.SendFollowingOrder();
            }
        }

        private ArmyPresentationModel GetArmy()
        {
            if (_armiesPool.Count > 0)
            {
                var fromPool = _armiesPool.Dequeue();
                fromPool.gameObject.SetActive(true);
                return fromPool;
            }

            var go = Instantiate(armyPrefab, transform);
            go.GetComponents(_armiesBuffer);

            var armyPresenter = _armiesBuffer.Single();
            armyPresenter.SetOwner(this);

            _armiesBuffer.Clear();

            return armyPresenter;
        }

        public void ReturnArmyToPool(ArmyPresentationModel armyPresentationModel)
        {
            Asserting.NotNull(limbo);

            if (armyPresentationModel.Army == null)
                return;

            armyPresentationModel.Army.Dispose();
            armyPresentationModel.Army = null;

            armyPresentationModel.gameObject.SetActive(false);
            armyPresentationModel.transform.position = limbo.position;
            _armiesPool.Enqueue(armyPresentationModel);
        }
    }
}