using Kugushev.Scripts.Common.Core.ValueObjects;

namespace Kugushev.Scripts.Common.Core.AI
{
    public interface IAgent
    {
        IOrder? CurrentOrder { get; set; }
        void ProcessCurrentOrder(DeltaTime delta);
    }
}