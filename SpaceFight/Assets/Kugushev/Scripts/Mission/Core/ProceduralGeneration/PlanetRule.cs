using System;
using Kugushev.Scripts.Mission.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Mission.ProceduralGeneration
{
    [Serializable]
    public class PlanetRule
    {
        [SerializeField] private PlanetSize planetSize;
        [SerializeField] private int minProduction;
        [SerializeField] private int maxProduction;

        public PlanetSize Size => planetSize;

        public int MinProduction => minProduction;

        public int MaxProduction => maxProduction;
    }
}