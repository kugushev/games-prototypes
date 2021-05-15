using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class Parliament
    {
        private IReadOnlyList<IPolitician>? _politicians;


        internal void Init(IReadOnlyList<IPolitician> politicians)
        {
            if (_politicians != null)
                throw new SpaceFightException("Double initialization of parliament");

            _politicians = politicians;
        }

        public IReadOnlyList<IPolitician> Politicians =>
            _politicians ?? throw new SpaceFightException("Parliament is not initialized");
    }
}