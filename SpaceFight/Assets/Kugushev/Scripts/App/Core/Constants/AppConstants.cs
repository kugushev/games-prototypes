using Kugushev.Scripts.Common;

namespace Kugushev.Scripts.App.Constants
{
    public static class AppConstants
    {
        public const string MenuPrefix = CommonConstants.MenuPrefix + "App/";

        public const int DefaultSeed = 42;

        public static class Scenes
        {
            public const string GameManagementScene = "GameManagementScene";
            public const string CampaignManagementScene = "CampaignManagementScene";
            public const string MainMenuScene = "MainMenuScene";
        }
    }
}