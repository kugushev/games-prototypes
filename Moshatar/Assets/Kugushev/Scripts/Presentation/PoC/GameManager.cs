using System;
using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class GameManager: MonoBehaviour
    {
        private void Awake()
        {
            Performance.TryGetAvailableDisplayRefreshRates(out var rates);

            if (rates.Length > 0)
            {
                var result = Performance.TrySetDisplayRefreshRate(rates.Max());
                if (!result)
                {
                    Debug.LogError($"Unable to set FPS {rates.Max()}");
                } 
            }
        }
    }
}