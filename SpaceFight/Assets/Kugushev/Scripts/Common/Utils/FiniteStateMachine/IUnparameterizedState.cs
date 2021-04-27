using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public interface IUnparameterizedState: IState
    {
        UniTask OnEnterAsync();
    }
}