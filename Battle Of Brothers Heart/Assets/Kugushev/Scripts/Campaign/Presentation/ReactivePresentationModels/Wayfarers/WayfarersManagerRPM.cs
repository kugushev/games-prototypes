using System;
using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels.Wayfarers
{
    public class WayfarersManagerRPM : MonoBehaviour
    {
        [Inject] private WayfarersManager _model = default!;
        [Inject] private BanditWayfarerRPM.Factory _banditsFactory = default!;

        private void Awake()
        {
            foreach (var bandit in _model.Bandits)
            {
                _banditsFactory.Create(bandit);
            }
        }
    }
}