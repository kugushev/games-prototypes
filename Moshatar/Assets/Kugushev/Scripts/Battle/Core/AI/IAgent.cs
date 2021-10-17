using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.AI
{
    public interface IAgent
    {
        IOrder? CurrentOrder { get; set; }
        void ProcessCurrentOrder(DeltaTime delta);
    }
}