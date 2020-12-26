using Kugushev.Scripts.Game.AI;

namespace Kugushev.Scripts.Game.Features
{
    public interface IActive
    {
        ICommander Commander { get; }
    }
}