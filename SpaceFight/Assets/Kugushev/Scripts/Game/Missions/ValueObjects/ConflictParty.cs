using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Interfaces;
using Kugushev.Scripts.Game.Missions.Managers;

namespace Kugushev.Scripts.Game.Missions.ValueObjects
{
    public readonly struct ConflictParty
    {
        public ConflictParty(Faction faction, Fleet fleet, ICommander commander)
        {
            Faction = faction;
            Fleet = fleet;
            Commander = commander;
        }

        public Faction Faction { get; }
        public Fleet Fleet { get; }
        public ICommander Commander { get; }

        public void Dispose()
        {
            Commander.WithdrawFleet();
            Fleet.Dispose();
        }
    }
}