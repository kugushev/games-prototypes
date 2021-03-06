using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.FiniteStateMachine
{
    public interface IState
    {
        UniTask OnEnterAsync();
        void OnUpdate(float deltaTime);
        UniTask OnExitAsync();
    }
}