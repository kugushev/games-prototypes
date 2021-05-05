using System.Collections.Generic;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public class IntriguesSet
    {
        private readonly List<IntrigueRecord> _intrigues = new List<IntrigueRecord>(16);


        public List<IntrigueRecord> Intrigues => _intrigues;
    }
}