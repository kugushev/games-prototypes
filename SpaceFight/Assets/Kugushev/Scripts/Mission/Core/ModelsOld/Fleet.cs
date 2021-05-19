using System.Collections.Generic;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Services;
using Kugushev.Scripts.Mission.Utils;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Models
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "FleetManager")]
    public class Fleet : ScriptableObject, IModel
    {
        [SerializeField] private ObjectsPool? pool;
        [SerializeField] private MissionModelProvider? modelProvider;
        [SerializeField] private MissionEventsCollector? eventsCollector;
        [SerializeField] private float armySpeed = GameplayConstants.ArmySpeed;
        [SerializeField] private float armyAngularSpeed = 1f;
        [SerializeField] private Faction faction;

        private FleetPerks? _fleetPerks;
        private FleetPerks? _emptyFleetPerks;

        public Queue<ArmyOld> ArmiesToSent { get; } = new Queue<ArmyOld>();

        public void SetFleetProperties(FleetPerks fleetPerks) => _fleetPerks = fleetPerks;
        public void ClearFleetProperties() => _fleetPerks = default;

        public void CommitOrder(Order order, Planet target)
        {
            Asserting.NotNull(modelProvider, pool, eventsCollector);

            if (!modelProvider.TryGetModel(out var model))
            {
                Debug.LogError("Unable to get model");
                return;
            }

            order.Commit(target);
            if (order.SourcePlanet.Power > 0 && order.SourcePlanet.TryRecruit(order.Power, out var power))
            {
                var army = pool.GetObject<ArmyOld, ArmyOld.State>(new ArmyOld.State(
                    order, armySpeed, armyAngularSpeed, faction, power,
                    in model.PlanetarySystem.GetSun(), GetFleetPerks(pool), eventsCollector));

                ArmiesToSent.Enqueue(army);
                eventsCollector.ArmySent.Add(new ArmySent(faction, power, order.SourcePlanet.Power));
            }
            else
            {
                Debug.LogWarning("Planet power is zero");
                order.Dispose();
            }
        }

        private FleetPerks GetFleetPerks(ObjectsPool objectsPool)
        {
            if (_fleetPerks != null)
                return _fleetPerks;

            _emptyFleetPerks ??=
                objectsPool.GetObject<FleetPerks, FleetPerks.State>(PlayerPropertiesServiceOld
                    .CreateDefaultFleetPerksState(objectsPool));

            return _emptyFleetPerks;
        }

        public void Dispose()
        {
            foreach (var army in ArmiesToSent)
                army.Dispose();
        }
    }
}