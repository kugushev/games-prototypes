using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionDataInitializer : IInitializable, ITransition
    {
        public bool ToTransition { get; private set; }

        public void Initialize()
        {
            ToTransition = true;
        }
    }
}