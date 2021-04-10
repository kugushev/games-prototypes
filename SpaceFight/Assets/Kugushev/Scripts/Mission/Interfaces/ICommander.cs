using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.Interfaces
{
    public interface ICommander
    {
        bool Surrendered { get; }
        void AssignFleet(Fleet fleet, Faction faction);
        void WithdrawFleet();
    }
}