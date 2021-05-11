using Kugushev.Scripts.Common;
using Kugushev.Scripts.Game.Core.ValueObjects;

namespace Kugushev.Scripts.Game.Core.Constants
{
    public static class GameConstants
    {
        public const string MenuPrefix = CommonConstants.MenuPrefix + "Game/";

        public const int ParliamentSize = 9;

        public const int PlayerCampaignBudget = 5;
        public const int MaxCampaignBudget = 15;

        public static float MinIncomeProbability = 0.5f;
        public static float MaxIncomeProbability = 1f;

        public const int PoliticianIncome = 3;
        public const int MinStartBudget = 1;
        public const int MaxStartBudget = 3;
        public const int MaxBudget = 5;

        public const int StartRelationLevel = 0;
        public const int MinRelationLevel = -10;
        public const int MaxRelationLevel = 10;

        public const int MinTraitValue = -3;
        public const int MaxTraitValue = 3;

        public const int LoyalPoliticsToWin = 1;

        public static readonly TraitsStatus StartTraitsStatus = new TraitsStatus();
    }
}