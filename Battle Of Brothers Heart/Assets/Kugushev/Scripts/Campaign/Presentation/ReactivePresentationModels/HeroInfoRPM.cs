using System;
using Kugushev.Scripts.Game.Core.Models;
using TMPro;
using UnityEngine;
using Zenject;
using Kugushev.Scripts.Common.Presentation.Utils;
using UniRx;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels
{
    public class HeroInfoRPM : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText = default!;

        [Inject] private Hero _model = default!;

        private void Awake()
        {
            _model.Gold.Select(value => value.ToString()).SubscribeToTextMeshPro(goldText).AddTo(this);
        }
    }
}