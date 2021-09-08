﻿using System.Collections;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class FireHeart : MonoBehaviour
    {
        private const float BurningRateSoftCap = 10f;
        private const int BurningRateHardCap = 20;
        private readonly WaitForSeconds _waitForColling = new WaitForSeconds(3);

        [SerializeField] private AudioSource burningSound;
        [SerializeField] private AudioSource overheatingSound;

        public ReactiveProperty<int> BurningRate { get; } = new ReactiveProperty<int>(0);

        private void Awake()
        {
            BurningRate.Subscribe(BurningRateChanged).AddTo(this);
        }

        private void Start()
        {
            StartCoroutine(Cooling());
        }

        private void BurningRateChanged(int value)
        {
            burningSound.volume = Mathf.Min(value / BurningRateSoftCap, 1f);
            if (value > BurningRateHardCap)
                overheatingSound.Play();
            else
                overheatingSound.Stop();
        }

        private IEnumerator Cooling()
        {
            while (true)
            {
                if (BurningRate.Value > 0)
                    BurningRate.Value -= 1;

                yield return _waitForColling;
            }
        }
    }
}