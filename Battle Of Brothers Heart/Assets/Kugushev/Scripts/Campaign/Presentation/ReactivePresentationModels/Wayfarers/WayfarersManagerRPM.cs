using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Game.Core;
using UnityEngine;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels.Wayfarers
{
    public class WayfarersManagerRPM : MonoBehaviour
    {
        [Inject] private Core.Models.Wayfarers.Wayfarers _model = default!;
        [Inject] private BanditWayfarerRPM.Factory _banditsFactory = default!;

        private readonly List<BanditWayfarerRPM> _bandits = new List<BanditWayfarerRPM>(
            GameConstants.World.BanditsPerCityMax *
            GameConstants.World.CitiesInVertical *
            GameConstants.World.CitiesInHorizontal);

        private void Awake()
        {
            foreach (var bandit in _model.Bandits)
                _bandits.Add(_banditsFactory.Create(bandit));

            _model.Bandits.ObserveAdd()
                .Subscribe(eventAdd => _bandits.Add(_banditsFactory.Create(eventAdd.Value)))
                .AddTo(this);

            _model.Bandits.ObserveRemove()
                .Subscribe(eventRemove => RemoveBandit(eventRemove.Value))
                .AddTo(this);
        }

        private void RemoveBandit(BanditWayfarer bandit)
        {
            BanditWayfarerRPM? found = null;
            foreach (var rpm in _bandits)
                if (rpm.Model == bandit)
                {
                    found = rpm;
                    break;
                }

            if (found == null)
            {
                Debug.LogError($"Bandit {bandit} not found in presenters");
                return;
            }

            _bandits.Remove(found);
            Destroy(found.gameObject);
        }
    }
}