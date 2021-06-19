using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Interfaces.AI
{
    public interface IAgent
    {
        IOrder? CurrentOrder { get; set; }
        void ProcessCurrentOrder(DeltaTime delta);
    }
}