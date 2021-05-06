using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Utils
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(CampaignSceneResultPipeline))]
    public class CampaignSceneResultPipeline : SceneParametersPipeline<CampaignResultInfo>
    {
    }
}