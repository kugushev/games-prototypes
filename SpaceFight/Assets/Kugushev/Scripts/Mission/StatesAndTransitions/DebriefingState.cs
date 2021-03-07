using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    public class DebriefingState : BaseSceneLoadingState<MissionModel>
    {
        // show winner + achievements

        public DebriefingState(MissionModel model)
            : base(model, UnityConstants.Scenes.MissionDebriefingScene, true)
        {
        }
    }
}