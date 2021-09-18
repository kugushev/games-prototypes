using System;
using System.Collections;
using Kugushev.Scripts.Presentation.PoC.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class HeroHeadController : MonoBehaviour
    {
        public const int MaxHitPoints = 100;

        private readonly WaitForSeconds _waitForHeal = new WaitForSeconds(1);

        [SerializeField] private VolumeProfile volumeProfile;

        private Vignette _vignette;

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        public ReactiveProperty<int> HitPoints { get; } = new ReactiveProperty<int>(MaxHitPoints);

        private IEnumerator Start()
        {
            HitPoints.Subscribe(HitChanged).AddTo(this);

            if (!volumeProfile.TryGet(out _vignette))
                Debug.LogError("No Vignette found");

            while (true)
            {
                if (HitPoints.Value < MaxHitPoints)
                    HitPoints.Value += 1;
                yield return _waitForHeal;
            }
        }

        private void HitChanged(int value)
        {
            if (_vignette != null && _vignette.intensity != null)
            {
                _vignette.intensity.value = 1f - (float)value / MaxHitPoints;
            }
        }

        private void OnDestroy()
        {
            if (_vignette != null && _vignette.intensity != null) 
                _vignette.intensity.value = 0;
        }
    }
}