namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public interface IParameterizedTransition<out T> : IReusableTransition
    {
        public T ExtractParameters();
    }
}