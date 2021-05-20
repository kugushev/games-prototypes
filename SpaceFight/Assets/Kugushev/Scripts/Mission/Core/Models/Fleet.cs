using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Core.Models.Effects;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Services;
using Kugushev.Scripts.Mission.ValueObjects.MissionEvents;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Core.Models
{
    public class GreenFleet : Fleet
    {
        public GreenFleet([JetBrains.Annotations.NotNull] Army.Factory armiesFactory,
            [JetBrains.Annotations.NotNull] EventsCollectingService eventsCollectingService) : base(armiesFactory,
            eventsCollectingService)
        {
        }

        protected override Faction Faction => Faction.Green;
    }

    public class RedFleet : Fleet
    {
        public RedFleet([JetBrains.Annotations.NotNull] Army.Factory armiesFactory,
            [JetBrains.Annotations.NotNull] EventsCollectingService eventsCollectingService)
            : base(armiesFactory, eventsCollectingService)
        {
        }

        protected override Faction Faction => Faction.Red;
    }

    public abstract class Fleet
    {
        private readonly Queue<Army> _armiesToSent = new Queue<Army>();
        private readonly Army.Factory _armiesFactory;
        private readonly EventsCollectingService _eventsCollectingService;
        private IFleetEffects? _fleetEffects;

        protected Fleet(Army.Factory armiesFactory, EventsCollectingService eventsCollectingService)
        {
            _armiesFactory = armiesFactory;
            _eventsCollectingService = eventsCollectingService;
        }

        public event Action? OrderCommitted;
        protected abstract Faction Faction { get; }

        public bool TryExtractArmy([NotNullWhen(true)] out Army? army)
        {
            if (_armiesToSent.Count > 0)
            {
                army = _armiesToSent.Dequeue();
                return true;
            }

            army = default;
            return false;
        }

        internal void SetFleetEffects(IFleetEffects effects)
        {
            if (_fleetEffects is { })
                throw new SpaceFightException("Fleet Effects are already set");

            _fleetEffects = effects;
        }

        internal void CommitOrder(OrderBuilder orderBuilder, Planet target)
        {
            var sourcePlanet = orderBuilder.SourcePlanet;
            if (sourcePlanet.Power.Value > 0 && sourcePlanet.TryRecruit(orderBuilder.Power, out var power))
            {
                var order = orderBuilder.Commit(target);

                var army = _armiesFactory.Create(order, Faction, new Power(power), GetFleetEffects());

                _armiesToSent.Enqueue(army);

                _eventsCollectingService.ArmySent.Add(new ArmySent(Faction, power, sourcePlanet.Power.Value));

                OrderCommitted?.Invoke();
            }
            else
            {
                Debug.LogWarning("Planet power is zero");
                orderBuilder.Rollback();
            }
        }

        private IFleetEffects GetFleetEffects()
        {
            if (_fleetEffects != null)
                return _fleetEffects;

            return FleetEffects.Empty;
        }
    }
}