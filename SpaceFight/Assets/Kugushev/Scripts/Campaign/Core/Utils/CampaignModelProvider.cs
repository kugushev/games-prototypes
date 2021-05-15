using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Models;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Utils
{
    [CreateAssetMenu(menuName = CampaignConstants.MenuPrefix + nameof(CampaignModelProvider))]
    public class CampaignModelProvider: ModelProvider<CampaignModelOld>
    {
        
    }
}