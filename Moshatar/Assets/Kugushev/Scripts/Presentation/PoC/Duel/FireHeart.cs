using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class FireHeart : MonoBehaviour
    {
        private const float BurningRateSoftCap = 10f;
        private const int BurningRateHardCap = 20;
        private readonly WaitForSeconds _waitForColling = new WaitForSeconds(3);

        [SerializeField] private AudioSource burningSound;
        [SerializeField] private AudioSource overheatingSound;

        [Inject] private HeroHeadController _heroHeadController;

        public ReactiveProperty<int> BurningRate { get; } = new ReactiveProperty<int>(100);
        public bool Breathing { get; set; }

        private void Awake()
        {
            BurningRate.Subscribe(BurningRateChanged).AddTo(this);
        }

        private void Start()
        {
            StartCoroutine(HeartControl());
        }

        private void BurningRateChanged(int value)
        {
            burningSound.volume = Mathf.Min(value / BurningRateSoftCap, 1f);
        }

        private IEnumerator HeartControl()
        {
            while (true)
            {
                if (BurningRate.Value > 0)
                    BurningRate.Value -= Breathing ? 3 : 1;

                if (BurningRate.Value > BurningRateHardCap)
                {
                    overheatingSound.Play();
                    _heroHeadController.HitPoints.Value -= 20;
                }

                yield return _waitForColling;
            }
        }
    }
}