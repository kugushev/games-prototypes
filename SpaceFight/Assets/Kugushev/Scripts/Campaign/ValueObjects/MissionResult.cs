using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionResult
    {
        public MissionResult(bool playerWin, AchievementInfo? reward)
        {
            PlayerWin = playerWin;
            Reward = reward;
        }

        public bool PlayerWin { get; }
        public AchievementInfo? Reward { get; }
    }
}