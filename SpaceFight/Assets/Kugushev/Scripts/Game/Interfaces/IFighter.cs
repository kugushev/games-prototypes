using Kugushev.Scripts.Game.Entities;

namespace Kugushev.Scripts.Game.Interfaces
{
    public interface IFighter
    {
        bool TryCapture(Army invader);
    }
}