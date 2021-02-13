using Kugushev.Scripts.Game.Missions.Entities;

namespace Kugushev.Scripts.Game.Missions.Interfaces
{
    public interface IFighter
    {
        bool TryCapture(Army invader);
    }
}