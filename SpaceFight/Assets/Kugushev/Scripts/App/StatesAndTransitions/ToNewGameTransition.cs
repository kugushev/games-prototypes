using Kugushev.Scripts.Common.Utils.FiniteStateMachine;

namespace Kugushev.Scripts.App.StatesAndTransitions
{
    public class ToNewGameTransition : IReusableTransition
    {
        public bool ToTransition { get; private set; }

        void IReusableTransition.Reset()
        {
            ToTransition = false;
        }

        public void OnNewGame() => ToTransition = true;
    }
}