using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters
{
    [CreateAssetMenu(fileName = "Hunter", menuName = "Game/Characters/Hunter", order = 0)]
    public class Hunter : Character
    {
        public override Faction Faction => Faction.Heroes;
    }
}