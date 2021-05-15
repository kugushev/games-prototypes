using JetBrains.Annotations;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Mission.Constants;

namespace Kugushev.Scripts.Mission.Core.ContextManagement
{
    public class BriefingState : UnparameterizedSceneLoadingState
    {
        public BriefingState() : base(UnityConstants.Scenes.MissionBriefingScene, true)
        {
        }
    }
}