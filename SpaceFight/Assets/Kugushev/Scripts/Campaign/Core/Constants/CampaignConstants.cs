using Kugushev.Scripts.Common;

namespace Kugushev.Scripts.Campaign.Constants
{
    public static class CampaignConstants
    {
        public const string MenuPrefix = CommonConstants.MenuPrefix + "Campaign/";

        public const int MissionSeedMin = 0;
        public const int MissionSeedMax = 100;
        
        public const int MissionsCount = NormalMissionsCount + HardMissionsCount + InsaneMissionsCount;
        public const int NormalMissionsCount = 7;
        public const int HardMissionsCount = 5;
        public const int InsaneMissionsCount = 3;

        public const int MissionCost = 1;
        public const int MaxBudget = 15;
    }
}