using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.Core.Models.Effects
{
    public class FleetEffects : IFleetEffects
    {
        public ValuePipeline<Army> SiegeDamage { get; } = new ValuePipeline<Army>();
        IValuePipeline<Army> IFleetEffects.SiegeDamage => SiegeDamage;

        public ValuePipeline<Army> FightDamage { get; } = new ValuePipeline<Army>();
        IValuePipeline<Army> IFleetEffects.FightDamage => FightDamage;

        public ValuePipeline<Army> FightProtection { get; } = new ValuePipeline<Army>();
        IValuePipeline<Army> IFleetEffects.FightProtection => FightProtection;

        public ValuePipeline<(Planet target, Faction playerFaction)> ArmySpeed { get; } =
            new ValuePipeline<(Planet target, Faction playerFaction)>();

        IValuePipeline<(Planet target, Faction playerFaction)> IFleetEffects.ArmySpeed => ArmySpeed;

        public float DeathStrike { get; set; }
        public SiegeUltimatum ToNeutralPlanetUltimatum { get; set; }
    }
}