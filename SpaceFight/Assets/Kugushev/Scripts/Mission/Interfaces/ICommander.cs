using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.Interfaces
{
    public interface ICommander
    {
        void AssignFleet(Fleet fleet, Faction faction);
        void WithdrawFleet();
    }
}