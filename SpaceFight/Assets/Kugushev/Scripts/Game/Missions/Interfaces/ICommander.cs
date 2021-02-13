using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Managers;

namespace Kugushev.Scripts.Game.Missions.Interfaces
{
    public interface ICommander
    {
        void AssignFleet(Fleet fleet, Faction faction);
        void WithdrawFleet();
    }
}