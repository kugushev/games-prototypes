using System;
using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters
{
    public class BattleControlPresenter : MonoBehaviour
    {
        [SerializeField] private Button increaseSpawn;
        [SerializeField] private Button decreaseSpawn;
        [SerializeField] private TextMeshProUGUI textReport;

        [Inject] private Director _director;

        private const int DefaultMax = 22;

        private void Start()
        {
            increaseSpawn.onClick.AddListener(HandleIncrease);
            decreaseSpawn.onClick.AddListener(HandleDecrease);
            textReport.text = StringBag.FromInt(DefaultMax);
        }

        private void OnDestroy()
        {
            increaseSpawn.onClick.AddListener(HandleIncrease);
            decreaseSpawn.onClick.AddListener(HandleDecrease);
        }

        private void HandleIncrease()
        {
            _director.MaxOverride ??= DefaultMax;
            _director.MaxOverride++;
            UpdateOutput();
        }

        private void HandleDecrease()
        {
            _director.MaxOverride ??= DefaultMax;
            _director.MaxOverride--;
            UpdateOutput();
        }

        private void UpdateOutput() => textReport.text = StringBag.FromInt(_director.MaxOverride ?? DefaultMax);
    }
}