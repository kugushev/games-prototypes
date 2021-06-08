using UniRx;

namespace Kugushev.Scripts.Core.Game.Models
{
    public class PlayerTeam
    {
        private readonly ReactiveCollection<PlayerTeamMember> _teamMembers = new ReactiveCollection<PlayerTeamMember>();

        public IReadOnlyReactiveCollection<PlayerTeamMember> TeamMembers => _teamMembers;

        public PlayerTeam()
        {
            _teamMembers.Add(new PlayerTeamMember());
        }
    }
}