using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Mission.ValueObjects;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class ToDebriefingTransition : ITransition
    {
        private readonly MissionModel _model;

        public ToDebriefingTransition(MissionModel model)
        {
            _model = model;
        }

        public bool ToTransition => IsMissionFinished(_model.PlanetarySystem);

        private static bool IsMissionFinished(PlanetarySystem planetarySystem)
        {
            bool greedIsAlive = false;
            bool redIsAlive = false;

            foreach (var planet in planetarySystem.Planets)
            {
                switch (planet.Faction)
                {
                    case Faction.Green:
                        greedIsAlive = true;
                        break;
                    case Faction.Red:
                        redIsAlive = true;
                        break;
                }

                if (greedIsAlive && redIsAlive)
                    return false;
            }

            return true;
        }
    }
}