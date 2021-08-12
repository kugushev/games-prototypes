using Kugushev.Scripts.Game.Models.HeroInfo;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.Game.UI
{
    public class Gold : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldValue;

        [Inject] private Hero _hero;

        private void Awake()
        {
            _hero.Gold.Subscribe(ChangeGold).AddTo(this);
        }

        private void ChangeGold(int gold)
        {
            goldValue.text = gold.ToString();
        }
    }
}