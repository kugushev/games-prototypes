using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Common.Models;
using Kugushev.Scripts.Common.Models.Abstractions;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Models
{
    [CreateAssetMenu(menuName = CampaignConstants.MenuPrefix + nameof(CampaignModelProvider))]
    internal class CampaignModelProvider: ModelProvider<CampaignModel, CampaignManager>
    {
        
    }
}