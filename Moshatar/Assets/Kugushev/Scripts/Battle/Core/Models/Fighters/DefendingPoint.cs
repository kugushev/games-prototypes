using Kugushev.Scripts.Battle.Core.ValueObjects;

namespace Kugushev.Scripts.Battle.Core.Models.Fighters
{
    public class DefendingPoint : BaseFighter
    {
        public DefendingPoint(Position battlefieldPosition, int hitPoints, Battlefield battlefield)
            : base(battlefieldPosition, new Character(hitPoints, 0), battlefield)
        {
        }
    }
}