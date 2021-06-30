using System.Collections.Generic;

namespace Kugushev.Scripts.Game.Core.Models
{
    public class Party
    {
        public Party(IReadOnlyList<Person> persons)
        {
            Persons = persons;
        }

        public IReadOnlyList<Person> Persons { get; }
    }
}