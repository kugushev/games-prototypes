using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Utils
{
    [CreateAssetMenu(menuName = CampaignConstants.MenuPrefix + nameof(MissionSceneParametersPipeline))]
    public class MissionSceneParametersPipeline: SceneParametersPipeline<MissionParameters>
    {
        
    }
}