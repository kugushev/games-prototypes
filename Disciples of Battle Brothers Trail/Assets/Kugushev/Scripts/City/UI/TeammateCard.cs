using System;
using Kugushev.Scripts.Game.Models.CityInfo;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.ProceduralGeneration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.City.UI
{
    public class TeammateCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private TextMeshProUGUI lvl;
        [SerializeField] private TextMeshProUGUI hp;
        [SerializeField] private TextMeshProUGUI damage;
        [SerializeField] private Button fireButton;

        private Teammate _teammate;
        private Hero _owner;

        private void Awake()
        {
            fireButton.onClick.AddListener(Fire);
        }

        public void Init(Teammate teammate, Hero owner)
        {
            _teammate = teammate;
            _owner = owner;
            
            var unit = teammate.BattleUnit;

            unitName.text = unit.Name;
            lvl.text = unit.Lvl.ToString();
            hp.text = unit.HitPoints.ToString();
            damage.text = unit.Damage.ToString();
        }

        private void Fire()
        {
            _owner.Fire(_teammate);
        }
    }
}