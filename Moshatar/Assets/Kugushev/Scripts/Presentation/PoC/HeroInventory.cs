using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Presentation.Common;
using TMPro;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class HeroInventory : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;

        public ReactiveProperty<int> Gold { get; } = new ReactiveProperty<int>(0);

        private void Awake()
        {
            Gold.Select(StringBag.FromInt).SubscribeToTextMeshPro(goldText).AddTo(this);
        }
    }
}