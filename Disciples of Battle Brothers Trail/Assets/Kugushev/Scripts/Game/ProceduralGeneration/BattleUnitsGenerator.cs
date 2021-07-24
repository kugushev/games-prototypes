using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.ProceduralGeneration
{
    public class BattleUnitsGenerator
    {
        public static BattleUnit Create(int lvl, AttackType attackType)
        {
            int hitPoints;
            int damage;
            switch (lvl, attackType)
            {
                case (1, AttackType.Mele):
                    hitPoints = 100;
                    damage = 25;
                    break;
                case (2, AttackType.Mele):
                    hitPoints = 150;
                    damage = 50;
                    break;
                case (3, AttackType.Mele):
                    hitPoints = 200;
                    damage = 75;
                    break;
                case (1, AttackType.Range):
                    hitPoints = 40;
                    damage = 25;
                    break;
                case (2, AttackType.Range):
                    hitPoints = 90;
                    damage = 40;
                    break;
                case (3, AttackType.Range):
                    hitPoints = 140;
                    damage = 70;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lvl), lvl, "Wrong lvl");
            }

            return new BattleUnit(GetRandomName(), lvl, attackType, hitPoints, damage);
        }

        public static int GetUnitPrice(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return 100;
                case 2:
                    return 300;
                case 3:
                    return 800;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lvl), lvl, "Wrong lvl");
            }
        }

        public static string GetRandomName() => _names[Random.Range(0, _names.Count)];

        private static IReadOnlyList<string> _names = new[]
        {
            "Addisyn",
            "Karlee",
            "Taylor",
            "Claudia",
            "Mercedes",
            "Lina",
            "Myla",
            "Leilani",
            "Maritza",
            "Gretchen",
            "Janet",
            "Sarai",
            "Selah",
            "Adrienne",
            "Adeline",
            "Camilla",
            "Reyna",
            "Clarissa",
            "Matilda",
            "Hadley",
            "Simone",
            "Elianna",
            "Maia",
            "Amiya",
            "Madelyn",
            "Kaia",
            "Cameron",
            "Lisa",
            "Amara",
            "Ingrid",
            "Caylee",
            "Rayna",
            "Linda",
            "Skyler",
            "Lia",
            "Madelynn",
            "Arabella",
            "Yazmin",
            "Harmony",
            "Eve",
            "Kylee",
            "Campbell",
            "Diya",
            "Helen",
            "Angie",
            "Kendra",
            "Jaelyn",
            "Avery",
            "Saige",
            "Cindy",
            "Reagan",
            "Trinity",
            "Maeve",
            "Katrina",
            "Janiah",
            "Amaris",
            "Alyssa",
            "Maren",
            "Livia",
            "Christine",
            "Selina",
            "Eva",
            "Amya",
            "Emilie",
            "Stella",
            "Nevaeh",
            "Autumn",
            "Amy",
            "Lesly",
            "Bailey",
            "Yaritza",
            "Giovanna",
            "Hallie",
            "Rosa",
            "Carissa",
            "Isla",
            "Abbie",
            "Alessandra",
            "Deborah",
            "Karina",
            "Theresa",
            "Destiny",
            "Leanna",
            "Faith",
            "Kayleigh",
            "Iyana",
            "Emmalee",
            "Amani",
            "Chana",
            "Arielle",
            "Cynthia",
            "Haley",
            "Kenya",
            "Laila",
            "Juliet",
            "Martha",
            "Ashanti",
            "Marlene",
            "Ainsley",
            "Adelyn"
        };
    }
}