using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Perks.Abstractions;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Perks.Epic
{
    [CreateAssetMenu(menuName = MenuName + nameof(Kamikaze))]
    public class Kamikaze : BasePerk
    {
        [SerializeField] private int level;
        [SerializeField] private int losses;
        [SerializeField] private float deathStrike;

        private PerkInfo? _info;

        public override PerkInfo Info => _info ??= new PerkInfo(
            PerkId.Kamikaze, level, PerkType.Epic, nameof(Kamikaze),
            $"Lose {losses} armies in fights",
            $"Armies makes {deathStrike} to one target before death");

        public override bool Check(MissionEventsCollector missionEvents, Faction faction, MissionModel model)
        {
            var deaths = 0;
            foreach (var missionEvent in missionEvents.ArmyDestroyedInFight)
            {
                if (missionEvent.Victim == faction)
                    deaths++;
            }

            return deaths >= losses;
        }

        public override void Apply(ref FleetPerks.State fleetPerks, ref PlanetarySystemPerks.State planetarySystemPerks)
        {
            fleetPerks.deathStrike += deathStrike;
        }
    }
}