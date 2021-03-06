using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public interface IState
    {
        UniTask OnEnterAsync();
        void OnUpdate(float deltaTime);
        UniTask OnExitAsync();
    }
}