using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils.ValuesProcessing;
using Kugushev.Scripts.Mission.Core.Interfaces.Effects;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Core.Models.Effects
{
    public class PlanetarySystemEffects : IPlanetarySystemEffects
    {
        public PlanetarySystemEffects(Faction playerFaction) => ApplicantFaction = playerFaction;

        public Faction ApplicantFaction { get; }

        public ValuePipeline<Planet> Production { get; } = new ValuePipeline<Planet>();
        IValuePipeline<Planet> IPlanetarySystemEffects.Production => Production;

        public ValuePipeline<Planet> Damage { get; } = new ValuePipeline<Planet>();
        IValuePipeline<Planet> IPlanetarySystemEffects.Damage => Damage;

        public Func<float, bool>? IsFreeRecruitment { get; set; }

        public Func<bool>? GetExtraPlanetOnStart { get; set; }
    }
}