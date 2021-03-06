using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.Manager
{
    public class ToMainMenuTransition: ITransition
    {
        public bool ToTransition => true;
    }
}