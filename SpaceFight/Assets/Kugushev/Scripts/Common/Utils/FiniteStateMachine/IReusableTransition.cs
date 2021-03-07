namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public interface IReusableTransition : ITransition
    {
        void Reset();
    }
}