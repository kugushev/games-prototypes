using System.Collections.Generic;

namespace Kugushev.Scripts.Game.Core.Models
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