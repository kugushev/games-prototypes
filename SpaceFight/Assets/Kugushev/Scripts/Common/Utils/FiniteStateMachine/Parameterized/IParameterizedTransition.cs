namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public interface IParameterizedTransition<out T> : ITransition
    {
        public T ExtractParameters();
    }
}