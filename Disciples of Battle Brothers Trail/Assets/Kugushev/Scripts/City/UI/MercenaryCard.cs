using System;
using Kugushev.Scripts.Game.Models.CityInfo;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.ProceduralGeneration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.City.UI
{
    public class MercenaryCard : MonoBehaviour
    {
        [SerializeField] private Button hire;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private TextMeshProUGUI lvl;
        [SerializeField] private TextMeshProUGUI hp;
        [SerializeField] private TextMeshProUGUI damage;
        [SerializeField] private TextMeshProUGUI price;

        public BattleUnit Unit { get; private set; }
        private HiringDeskInfo _hiringDeskInfo;
        private Hero _hero;
        private int _price;

        public void Init(BattleUnit unit, Hero hero, HiringDeskInfo hiringDeskInfo)
        {
            _hero = hero;
            Unit = unit;
            _price = BattleUnitsGenerator.GetUnitPrice(unit.Lvl);
            _hiringDeskInfo = hiringDeskInfo;

            unitName.text = unit.Name;
            lvl.text = unit.Lvl.ToString();
            hp.text = unit.HitPoints.ToString();
            damage.text = unit.Damage.ToString();
            price.text = _price.ToString();
        }

        private void Awake()
        {
            hire.onClick.AddListener(OnHire);
        }

        private void OnHire()
        {
            _hero.TryHire(Unit, _price, _hiringDeskInfo);
        }
    }
}