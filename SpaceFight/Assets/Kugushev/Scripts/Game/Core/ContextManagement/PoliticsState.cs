﻿using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Constants;

namespace Kugushev.Scripts.Game.Core.ContextManagement
{
    public class PoliticsState : UnparameterizedSceneLoadingState
    {
        protected PoliticsState()
            : base(UnityConstants.PoliticsMenuScene, true)
        {
        }
    }
}