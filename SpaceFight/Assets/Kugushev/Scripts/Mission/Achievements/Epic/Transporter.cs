using System;
using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Mission.Achievements.Abstractions;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Transporter))]
    public class Transporter : EpicAchievement, IMultiplierPerk<(Planet target, Faction playerFaction)>
    {
        [SerializeField] private float totalPower;
        [SerializeField] private float acceleration;

        protected override AchievementId AchievementId => AchievementId.Transporter;
        protected override string Name => nameof(Transporter);

        protected override string Criteria => $"Transfer {totalPower} total power between your planets";

        protected override string Perk =>
            $"Increase armies speed in {acceleration} times of traveling between your planets";

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            float total = 0;
            foreach (var armyArrived in missionEvents.ArmyArrived)
            {
                if (armyArrived.Owner == faction)
                    total += armyArrived.Power;
            }

            return total >= totalPower;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            fleetPerks.armySpeed.AddPerk(this);
        }

        public float? GetMultiplier((Planet target, Faction playerFaction) criteria)
        {
            if (criteria.target.Faction == criteria.playerFaction)
                return acceleration;
            return null;
        }
    }
}