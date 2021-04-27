using Cysharp.Threading.Tasks;

namespace Kugushev.Scripts.Common.Utils.FiniteStateMachine
{
    public interface ITransitionRecord
    {
        IState Target { get; }
        bool ToTransition { get; }
        UniTask EnterState();
        void ResetTransition();
    }
}