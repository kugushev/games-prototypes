using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;

namespace Kugushev.Scripts.Game.Missions.Interfaces
{
    public interface IFighter
    {
        Faction Faction { get; }
        bool TryCapture(Army invader);
    }
}