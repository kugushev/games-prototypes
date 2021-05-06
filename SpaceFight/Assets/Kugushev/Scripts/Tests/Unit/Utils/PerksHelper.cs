using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models.Effects;
using Kugushev.Scripts.Mission.Services;
using UnityEngine;

namespace Kugushev.Scripts.Tests.Unit.Utils
{
    public static class PerksHelper
    {
        public static (PlanetarySystemPerks, FleetPerks) GetPlayerProperties(
            params (PerkId, int? level, PerkType)[] perks)
        {
            var service = AssetBundleHelper.LoadAsset<PlayerPropertiesService>("Test Player Properties Service");

            var playerPerks = ScriptableObject.CreateInstance<ObjectsPool>()
                .GetObject<PlayerPerks, PlayerPerks.State>(new PlayerPerks.State(PerkIdHelper.AllPerks));
            foreach (var (perkId, level, perkType) in perks)
            {
                playerPerks.AddPerk(new PerkInfo(perkId, level, perkType, "", "", ""));
            }

            return service.GetPlayerProperties(Faction.Green, new MissionParameters(default, playerPerks));
        }
    }
}