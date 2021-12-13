using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class HeroFighter : BaseFighter
    {
        private const int MaxHp = 300;

        protected override bool SimplifiedSuffering => true;

        public HeroFighter(Position battlefieldPosition, Battlefield battlefield)
            : base(battlefieldPosition, new Character(MaxHp, 0), battlefield)
        {
        }

        public void UpdatePosition(Position position)
        {
            PositionImpl.Value = position;
        }
    }
}