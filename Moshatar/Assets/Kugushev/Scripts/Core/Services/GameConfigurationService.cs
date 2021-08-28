using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Core.Services
{
    public class GameConfigurationService : IInitializable
    {
        public void Initialize()
        {
            Performance.TryGetAvailableDisplayRefreshRates(out var rates);

            if (rates.Length > 0)
            {
                var result = Performance.TrySetDisplayRefreshRate(rates.Max());
                if (!result)
                    Debug.LogError($"Unable to set refresh rate {rates.Max()}");
            }
            else
                Debug.LogError($"No refresh rates found");
        }
    }
}