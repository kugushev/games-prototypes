using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(PoliticianCharacter))]
    public class PoliticianCharacter : ScriptableObject
    {
        [SerializeField] private string fullName;
        [SerializeField] private Perk perksLvl1;
        [SerializeField] private Perk perksLvl2;
        [SerializeField] private Perk perksLvl3;

        public string FullName => fullName;

        public PerkInfo PerkLvl1 => perksLvl1.Info;
        public PerkInfo PerkLvl2 => perksLvl2.Info;
        public PerkInfo PerkLvl3 => perksLvl3.Info;
    }
}