using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.Interfaces
{
    public interface IFighter
    {
        bool TryCapture(Army invader);
    }
}