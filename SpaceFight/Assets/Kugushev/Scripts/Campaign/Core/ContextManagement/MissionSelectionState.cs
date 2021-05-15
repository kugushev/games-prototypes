using JetBrains.Annotations;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Common.ContextManagement;

namespace Kugushev.Scripts.Campaign.Core.ContextManagement
{
    public class MissionSelectionState: UnparameterizedSceneLoadingState
    {
        public MissionSelectionState() 
            : base(UnityConstants.MissionSelectionScene, true)
        {
        }
    }
}