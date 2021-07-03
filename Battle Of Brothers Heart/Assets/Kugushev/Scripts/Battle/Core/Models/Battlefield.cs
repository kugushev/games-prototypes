using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Fighters;

namespace Kugushev.Scripts.Battle.Core.Models
{
    public class Battlefield
    {
        private readonly List<BaseFighter> _units = new List<BaseFighter>(8);

        public IEnumerable<BaseFighter> Units => _units.Where(u => !u.IsDead);

        public void RegisterUnt(BaseFighter fighter) => _units.Add(fighter);
    }
}