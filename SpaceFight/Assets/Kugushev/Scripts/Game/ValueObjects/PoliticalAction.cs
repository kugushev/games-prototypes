using System;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(PoliticalAction))]
    public class PoliticalAction : ScriptableObject
    {
        [SerializeField] private string caption;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private int intel;

        [Header("Traits")] [SerializeField] private int business;
        [SerializeField] private int greed;
        [SerializeField] private int lust;
        [SerializeField] private int brute;
        [SerializeField] private int vanity;

        private Traits? _traits;
        private Traits? _sideEffect;

        public string Caption => caption;
        public Difficulty Difficulty => difficulty;
        public int Intel => intel;
        public Traits Traits => _traits ??= new Traits(business, greed, lust, brute, vanity);


        public Traits SideEffect => _sideEffect ??= new Traits(
            GetOneOrNull(business),
            GetOneOrNull(greed),
            GetOneOrNull(lust),
            GetOneOrNull(brute),
            GetOneOrNull(vanity));

        private int GetOneOrNull(int trait) => trait != 0 ? trait / Math.Abs(trait) : 0;
    }
}