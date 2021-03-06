namespace Kugushev.Scripts.Game.ValueObjects
{
    public readonly struct CampaignInfo
    {
        public CampaignInfo(int seed)
        {
            Seed = seed;
        }

        public int Seed { get; }
    }
}