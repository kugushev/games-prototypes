using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Core.Battle.Models.Units;

namespace Kugushev.Scripts.Core.Battle.Models
{
    public class Battlefield
    {
        private readonly List<BaseUnit> _units = new List<BaseUnit>(8);

        public IEnumerable<BaseUnit> Units => _units.Where(u => !u.IsDead);

        public void RegisterUnt(BaseUnit unit) => _units.Add(unit);
    }
}