namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionResult
    {
        public MissionResult(bool playerWin)
        {
            PlayerWin = playerWin;
        }

        public bool PlayerWin { get; }
    }
}