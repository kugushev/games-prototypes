using UnityEngine;

namespace Kugushev.Scripts.Core.Models
{
    public class Score
    {
        public int LastGold { get; private set; }
        public int TopGold { get; private set; }

        public void Register(int gold)
        {
            LastGold = gold;
            TopGold = Mathf.Max(TopGold, LastGold);
        }
    }
}