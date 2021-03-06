using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.FiniteStateMachine
{
    public interface IState
    {
        bool CanEnter { get; }
        UniTask OnEnterAsync();
        void OnUpdate(float deltaTime);
        UniTask OnExitAsync();
    }
}