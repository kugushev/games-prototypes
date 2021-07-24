using System.Linq;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.ProceduralGeneration;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.CityInfo
{
    public class City
    {
        // todo: ugly code. Replace with parameters passing
        public static City VisitedCity { get; set; }

        public HiringDeskInfo HiringDeskInfo { get; } = new HiringDeskInfo();


    }
}