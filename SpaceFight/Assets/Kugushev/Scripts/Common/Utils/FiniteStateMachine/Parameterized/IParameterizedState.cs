using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized
{
    public interface IParameterizedState<in T> : IState
    {
        UniTask OnEnterAsync(T parameters);
    }
}