using Kugushev.Scripts.Game.Models.CityInfo;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.ProceduralGeneration;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.City.UI
{
    public class TeammateCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private TextMeshProUGUI lvl;
        [SerializeField] private TextMeshProUGUI hp;
        [SerializeField] private TextMeshProUGUI damage;

        public void Init(Teammate teammate)
        {
            var unit = teammate.BattleUnit;

            unitName.text = unit.Name;
            lvl.text = unit.Lvl.ToString();
            hp.text = unit.HitPoints.ToString();
            damage.text = unit.Damage.ToString();
        }
    }
}