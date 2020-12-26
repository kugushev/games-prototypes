using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.Characters.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters
{
    [CreateAssetMenu(fileName = "Dummy", menuName = "Game/Characters/Dummy")]
    public class Dummy : Character
    {
        public override Faction Faction => Faction.Greenskin;
    }
}