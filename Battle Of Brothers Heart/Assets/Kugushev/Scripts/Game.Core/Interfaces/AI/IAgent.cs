using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.AI
{
    public interface IAgent
    {
        IOrder? CurrentOrder { get; set; }
        void ProcessCurrentOrder(DeltaTime delta);
    }
}