using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters
{
    [CreateAssetMenu(fileName = "Orc", menuName = "Game/Characters/Orc")]
    public class Orc: Character
    {
        public override Faction Faction => Faction.Greenskin;
    }
}