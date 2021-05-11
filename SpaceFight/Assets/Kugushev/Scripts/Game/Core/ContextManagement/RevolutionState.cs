using JetBrains.Annotations;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Constants;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    public class RevolutionState : UnparameterizedSceneLoadingState
    {
        public RevolutionState() : base(UnityConstants.RevolutionScene, true)
        {
        }
    }
}