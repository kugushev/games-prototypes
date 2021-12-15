using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Battle.Core.Models.Fighters;

namespace Kugushev.Scripts.Battle.Core.Models
{
    public class Battlefield
    {
        private readonly List<BaseFighter> _units = new List<BaseFighter>(8);

        public IEnumerable<BaseFighter> Units => _units.Where(u => !u.IsDead);

        private readonly List<BaseFighter> _unitsToDelete = new List<BaseFighter>(32);
        
        public void RegisterUnt(BaseFighter fighter)
        {
            foreach (var unit in _units)
                if (unit.IsDead)
                    _unitsToDelete.Add(unit);
            foreach (var unit in _unitsToDelete)
                _units.Remove(unit);

            
            _units.Add(fighter);
        }
    }
}