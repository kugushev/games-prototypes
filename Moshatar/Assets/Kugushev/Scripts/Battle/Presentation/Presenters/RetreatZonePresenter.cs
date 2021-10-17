using System;
using System.Globalization;
using Kugushev.Scripts.Battle.Core.Models;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Presenters
{
    public class RetreatZonePresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI retreatElapsed = default!;

        [Inject] private BattleSupervisor _battleSupervisor = default!;

        private int _heroesInZone;

        private void Awake()
        {
            _battleSupervisor.RetreatElapsed.Subscribe(UpdateElapsedTime).AddTo(this);
        }

        private void UpdateElapsedTime(TimeSpan? elapsed)
        {
            if (elapsed != null)
            {
                retreatElapsed.enabled = true;
                retreatElapsed.text = elapsed.Value.Seconds.ToString(CultureInfo.InvariantCulture);
            }
            else
                retreatElapsed.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Hero"))
            {
                _battleSupervisor.ToggleRetreat(true);
                _heroesInZone++;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Hero"))
            {
                _heroesInZone--;
                if (_heroesInZone <= 0)
                    _battleSupervisor.ToggleRetreat(false);
            }
        }
    }
}