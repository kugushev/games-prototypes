using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class ToDebriefingTransition : ITransition
    {
        private readonly MissionModel _model;

        public ToDebriefingTransition(MissionModel model)
        {
            _model = model;
        }

        public bool ToTransition => IsMissionFinished(_model.PlanetarySystem, out _);

        public static bool IsMissionFinished(PlanetarySystem planetarySystem, out Faction winner)
        {
            winner = Faction.Unspecified;
            
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
            
            winner = (greedIsAlive, redIsAlive) switch
            {
                (true, false) => Faction.Green,
                (false, true) => Faction.Red,
                (false, false) => Faction.Neutral,
                _ => Faction.Unspecified
            };

            return true;
        }
    }
}