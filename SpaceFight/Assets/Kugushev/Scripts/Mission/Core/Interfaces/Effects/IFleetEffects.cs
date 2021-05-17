using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ValueObjects;
using Planet = Kugushev.Scripts.Mission.Core.Models.Planet;

namespace Kugushev.Scripts.Mission.Core.Interfaces.Effects
{
    public interface IFleetEffects
    {
        IValuePipeline<Army> SiegeDamage { get; }
        IValuePipeline<Army> FightDamage { get; }
        IValuePipeline<Army> FightProtection { get; }
        IValuePipeline<(Planet target, Faction playerFaction)> ArmySpeed { get; }
        float DeathStrike { get; }
        SiegeUltimatum ToNeutralPlanetUltimatum { get; }
    }
}