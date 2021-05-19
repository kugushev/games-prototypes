using System.Collections.Generic;
using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Core.ValueObjects
{
    public class IntriguesSet
    {
        private readonly List<IntrigueCard> _intrigues = new List<IntrigueCard>(16);


        public List<IntrigueCard> Intrigues => _intrigues;
    }
}