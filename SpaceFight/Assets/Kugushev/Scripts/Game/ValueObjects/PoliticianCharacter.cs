using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(PoliticianCharacter))]
    public class PoliticianCharacter : ScriptableObject
    {
        [SerializeField] private string? fullName;
        [SerializeField] private Perk? perksLvl1;
        [SerializeField] private Perk? perksLvl2;
        [SerializeField] private Perk? perksLvl3;

        public string FullName
        {
            get
            {
                Asserting.NotNull(fullName);
                return fullName;
            }
        }

        public PerkInfo PerkLvl1
        {
            get
            {
                Asserting.NotNull(perksLvl1);
                return perksLvl1.Info;
            }
        }

        public PerkInfo PerkLvl2
        {
            get
            {
                Asserting.NotNull(perksLvl2);
                return perksLvl2.Info;
            }
        }

        public PerkInfo PerkLvl3
        {
            get
            {
                Asserting.NotNull(perksLvl3);
                return perksLvl3.Info;
            }
        }
    }
}