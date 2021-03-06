using Kugushev.Scripts.Mission.Entities;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Interfaces
{
    public interface ICommander
    {
        void AssignFleet(Fleet fleet, Faction faction);
        void WithdrawFleet();
    }
}