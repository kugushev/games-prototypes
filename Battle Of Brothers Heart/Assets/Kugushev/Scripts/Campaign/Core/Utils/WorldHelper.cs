using Kugushev.Scripts.Campaign.Core;

namespace Kugushev.Scripts.Campaign.Presentation.Helpers
{
    public static class WorldHelper
    {
        public static int NormalizeX(int x) => x - CampaignConstants.World.Width / 2;
        public static int NormalizeY(int y) => y - CampaignConstants.World.Height / 2;
    }
}