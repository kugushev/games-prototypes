using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public interface IState
    {
        void OnUpdate(float deltaTime);
        UniTask OnExitAsync();
    }
}