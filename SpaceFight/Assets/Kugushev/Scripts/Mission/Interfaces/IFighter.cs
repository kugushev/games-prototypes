using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.Interfaces
{
    public interface IFighter
    {
        bool Active { get; }
        Position Position { get; }
        Faction Faction { get; }
        bool CanBeAttacked { get; }
    }
}