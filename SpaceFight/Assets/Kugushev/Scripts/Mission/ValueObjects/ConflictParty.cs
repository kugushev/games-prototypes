using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Interfaces;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.ValueObjects
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