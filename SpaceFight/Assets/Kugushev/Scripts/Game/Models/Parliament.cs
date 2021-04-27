using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Interfaces;

namespace Kugushev.Scripts.Game.Models
{
    public class Parliament
    {
        private readonly List<Politician> _politicians;

        internal Parliament(List<Politician> politicians)
        {
            _politicians = politicians;
        }

        public IReadOnlyList<IPolitician> Politicians => _politicians;
    }
}