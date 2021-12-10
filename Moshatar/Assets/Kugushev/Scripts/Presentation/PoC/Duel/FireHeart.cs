﻿using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class FireHeart : MonoBehaviour
    {
        private const float BurningRateSoftCap = 20f;
        public const int BurningRateHardCap = 40;
        private readonly WaitForSeconds _waitForColling = new WaitForSeconds(1);

        [SerializeField] private AudioSource burningSound;
        [SerializeField] private AudioSource overheatingSound;

        [Inject] private HeroHeadController _heroHeadController;

        public ReactiveProperty<int> BurningRate { get; } = new ReactiveProperty<int>(0);
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
                    BurningRate.Value -= Breathing ? 4 : 1;

                // todo: we don't need overheating
                // if (BurningRate.Value > BurningRateHardCap)
                // {
                //     overheatingSound.Play();
                //     _heroHeadController.HitPoints.Value -= 20;
                // }

                yield return _waitForColling;
            }
        }
    }
}