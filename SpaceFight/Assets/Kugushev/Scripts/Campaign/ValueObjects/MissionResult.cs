using Kugushev.Scripts.App.ValueObjects;

namespace Kugushev.Scripts.Campaign.ValueObjects
{
    public readonly struct MissionResult
    {
        public MissionResult(bool playerWins, MissionInfo missionInfo, AchievementInfo? reward)
        {
            PlayerWins = playerWins;
            MissionInfo = missionInfo;
            Reward = reward;
        }

        public bool PlayerWins { get; }
        public MissionInfo MissionInfo { get; }
        public AchievementInfo? Reward { get; }
    }
}