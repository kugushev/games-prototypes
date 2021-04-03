using Kugushev.Scripts.Common;

namespace Kugushev.Scripts.Game.Constants
{
    public static class GameConstants
    {
        public const string MenuPrefix = CommonConstants.MenuPrefix + "Game/";

        public const int PlayerCampaignBudget = 5;
        public const int MaxCampaignBudget = 15;

        public const int PoliticianIncome = 2;
        public const int MaxStartBudget = 4;
        public const int MaxBudget = 5;

        public const int StartRelationLevel = 0;
        public const int MinRelationLevel = -10;
        public const int MaxRelationLevel = 10;

        public const int MinTraitValue = -3;
        public const int MaxTraitValue = 3;

        public const int LoyalPoliticsToWin = 5;
    }
}