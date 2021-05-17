using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Core.Interfaces.Effects
{
    public interface IPlanetarySystemEffects
    {
        Faction ApplicantFaction { get; }
        IValuePipeline<Planet> Production { get; }
        IValuePipeline<Planet> Damage { get; }
        Func<float, bool>? IsFreeRecruitment { get; }
        Func<bool>? GetExtraPlanetOnStart { get; }
    }
}