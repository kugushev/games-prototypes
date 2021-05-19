using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public class GreenFleet : Fleet
    {
        public override Faction Faction => Faction.Green;
    }

    public class RedFleet : Fleet
    {
        public override Faction Faction => Faction.Red;
    }

    public abstract class Fleet
    {
        public abstract Faction Faction { get; }

        private IFleetEffects? _fleetEffects;

        internal void SetFleetEffects(IFleetEffects effects)
        {
            if (_fleetEffects is { })
                throw new SpaceFightException("Fleet Effects are already set");

            _fleetEffects = effects;
        }
        
        public Queue<Army> ArmiesToSent { get; } = new Queue<Army>();

        // internal void CommitOrder(Order order, Planet target)
        // {
        //     Asserting.NotNull(modelProvider, pool, eventsCollector);
        //
        //     if (!modelProvider.TryGetModel(out var model))
        //     {
        //         Debug.LogError("Unable to get model");
        //         return;
        //     }
        //
        //     order.Commit(target);
        //     if (order.SourcePlanet.Power > 0 && order.SourcePlanet.TryRecruit(order.Power, out var power))
        //     {
        //         var army = pool.GetObject<Army, Army.State>(new Army.State(
        //             order, armySpeed, armyAngularSpeed, faction, power,
        //             in model.PlanetarySystem.GetSun(), GetFleetPerks(pool), eventsCollector));
        //
        //         ArmiesToSent.Enqueue(army);
        //         eventsCollector.ArmySent.Add(new ArmySent(faction, power, order.SourcePlanet.Power));
        //     }
        //     else
        //     {
        //         Debug.LogWarning("Planet power is zero");
        //         order.Dispose();
        //     }
        // }

        // private FleetPerks GetFleetPerks(ObjectsPool objectsPool)
        // {
        //     if (_fleetPerks != null)
        //         return _fleetPerks;
        //
        //     _emptyFleetPerks ??=
        //         objectsPool.GetObject<FleetPerks, FleetPerks.State>(PlayerPropertiesServiceOld
        //             .CreateDefaultFleetPerksState(objectsPool));
        //
        //     return _emptyFleetPerks;
        // }
    }
}