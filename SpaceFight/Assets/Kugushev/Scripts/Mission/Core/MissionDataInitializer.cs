using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Core.Models;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionDataInitializer : IInitializable, ITransition
    {
        private readonly PlanetarySystem _planetarySystem;

        public MissionDataInitializer(PlanetarySystem planetarySystem)
        {
            _planetarySystem = planetarySystem;
        }

        public bool ToTransition { get; private set; }

        public void Initialize()
        {
            //_planetarySystem.Init();

            ToTransition = true;
        }
    }
}