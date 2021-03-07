using Kugushev.Scripts.Common.StatesAndTransitions;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;
using UnityEngine;

namespace Kugushev.Scripts.Mission.StatesAndTransitions
{
    internal class BriefingState : BaseSceneLoadingState<MissionModel>
    {
        public BriefingState(MissionModel model)
            : base(model, UnityConstants.Scenes.MissionBriefingScene, true)
        {
        }

        protected override void AssertModel()
        {
            if (Model.PlanetarySystem == null)
                Debug.LogError("PlanetarySystem is not set");
        }
    }
}