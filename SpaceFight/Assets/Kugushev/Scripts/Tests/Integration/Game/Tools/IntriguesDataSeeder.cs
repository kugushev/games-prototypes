using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Tests.Integration.Game.Tools
{
    public class IntriguesDataSeeder : MonoBehaviour
    {
        [SerializeField] private Intrigue[] intrigues = default!;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private ObtainIntrigueCard.Factory _obtainIntrigueCardFactory = default!;

        private void Start()
        {
            foreach (var intrigue in intrigues)
            {
                for (int i = 0; i < 10; i++)
                {
                    _signalBus.Fire(_obtainIntrigueCardFactory.Create(intrigue));
                }
            }
        }
    }
}