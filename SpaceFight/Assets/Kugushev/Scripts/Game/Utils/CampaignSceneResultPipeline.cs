﻿using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Utils
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(CampaignSceneResultPipeline))]
    public class CampaignSceneResultPipeline : SceneParametersPipeline<CampaignResultInfo>
    {
    }
}