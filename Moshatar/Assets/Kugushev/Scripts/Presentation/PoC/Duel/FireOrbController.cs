using System;
using Kugushev.Scripts.Presentation.PoC.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class FireOrbController : MonoBehaviour
    {
        private const float MinVelocity = 0.015f;
        private const float HardVelocity = 0.1f;
        private const float ProjectileSpeed = 10f;

        [SerializeField] private HandMoveConvolution handMoveConvolution;
        [SerializeField] private GameObject vfx;
        [SerializeField] private ParticleSystem frontEffect;
        [SerializeField] private ParticleSystem dragonBreathEffect;
        [SerializeField] private AudioSource dragonBreathSound;
        [SerializeField] private BoxCollider dragonBreathCollider;
        [SerializeField] private AudioSource frontEffectSound;

        [Inject] private HeroHeadController _heroHeadController;
        [Inject] private FireHeart _fireHeart;
        [Inject] private BigProjectile.Factory _playerBigProjectile;
        [Inject] private SmallProjectile.Factory _playerSmallProjectile;

        private bool _touchingHead;

        private void Awake()
        {
            handMoveConvolution.MoveStart += HandMoveConvolutionOnMoveStart;
            handMoveConvolution.MoveFinished += HandMoveConvolutionOnMoveFinished;
        }

        private void OnDestroy()
        {
            handMoveConvolution.MoveStart -= HandMoveConvolutionOnMoveStart;
            handMoveConvolution.MoveFinished -= HandMoveConvolutionOnMoveFinished;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MainCamera"))
                _touchingHead = true;

            if (handMoveConvolution.Moving && _touchingHead)
                HandleDragonBreathOn();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("MainCamera"))
            {
                _touchingHead = false;
                if (handMoveConvolution.Moving)
                    vfx.SetActive(true);
                HandleDragonBreathOff();
            }
        }

        private void Update()
        {
            if (dragonBreathEffect.isPlaying)
            {
                dragonBreathEffect.transform.rotation = _heroHeadController.Rotation;
            }
        }

        private void HandMoveConvolutionOnMoveStart(HandMoveInfo startInfo)
        {
            if (_touchingHead)
                HandleDragonBreathOn();

            vfx.SetActive(true);
        }

        private void HandMoveConvolutionOnMoveFinished(HandMoveInfo startInfo, HandMoveInfo finishInfo)
        {
            HandleDragonBreathOff();

            vfx.SetActive(false);

            Recognize(startInfo, finishInfo);
        }

        private void Recognize(HandMoveInfo startInfo, HandMoveInfo finishInfo)
        {
            var actualVector = finishInfo.Position - startInfo.Position;

            const int burningRateIncrease = 1;
            if (handMoveConvolution.Velocity > HardVelocity)
            {
                _playerBigProjectile.Create(finishInfo.Position, actualVector, ProjectileSpeed);
                if (_fireHeart.BurningRate.Value < FireHeart.BurningRateHardCap)
                    _fireHeart.BurningRate.Value += burningRateIncrease;
            }
            else if (handMoveConvolution.Velocity > MinVelocity)
            {
                _playerSmallProjectile.Create(finishInfo.Position, actualVector, ProjectileSpeed);
                if (_fireHeart.BurningRate.Value < FireHeart.BurningRateHardCap)
                    _fireHeart.BurningRate.Value += burningRateIncrease;
            }

            // var headPosition = _heroHeadController.Position;
            //
            // var expectedStartPoint = new Vector3(headPosition.x, startinfo.Position.y, headPosition.z);
            //
            // var expectedVector = finishinfo.Position - expectedStartPoint;
            // var actualVector = finishinfo.Position - startinfo.Position;
            //
            // // Debug.DrawLine(expectedStartPoint, finishinfo.Position, Color.green, 15f);
            // // Debug.DrawLine(startinfo.Position, finishinfo.Position, Color.red, 15f);
            //
            // // if (handMoveConvolution.Velocity > MinVelocity)
            // //     _playerBigProjectile.Create(finishinfo.Position, actualVector, ProjectileSpeed);
            // // else
            // //     print($"No enough velocity: {handMoveConvolution.Velocity}");
            //
            // var cos = Vector3.Dot(expectedVector.normalized, actualVector.normalized);
            //
            // const float cos45Deg = 0.7f;
            // // if (cos > 0)
            // // {
            // // var deltaTime = Convert.ToSingle((finishinfo.Time - startinfo.Time).TotalMilliseconds);
            // // var deltaDistance = actualVector.magnitude;
            // //
            // // var avgHandSpeed = deltaDistance / deltaTime;
            //
            //
            // // }
            // // else
            // // {
            // //     //if (handMoveConvolution.Velocity > MinVelocity)
            // //     {
            // //         var spawnPoint = (finishinfo.Position + startinfo.Position) / 2;
            // //
            // //         var expectedHeart = new Vector3(headPosition.x, headPosition.y * 0.8f, headPosition.z);
            // //         var direction = (spawnPoint - expectedHeart).normalized;
            // //         var euler = new Vector3(
            // //             Mathf.Acos(direction.x) * Mathf.Rad2Deg,
            // //             Mathf.Acos(direction.y) * Mathf.Rad2Deg - 90f,
            // //             Mathf.Acos(direction.z) * Mathf.Rad2Deg
            // //         );
            // //
            // //         frontEffect.Stop();
            // //         frontEffect.transform.position = spawnPoint;
            // //         frontEffect.transform.rotation = Quaternion.Euler(euler);
            // //         // frontEffect.transform.LookAt(finishinfo.Position + startinfo.Position);
            // //         frontEffect.Play();
            // //
            // //         frontEffectSound.Stop();
            // //         frontEffectSound.Play();
            // //     }
            // //     // else
            // //     //     print("No enough velocity");
            // // }
        }

        private IDisposable _dragonSubs;

        private void HandleDragonBreathOn()
        {
            if (_fireHeart.BurningRate.Value > 0)
            {
                _dragonSubs = _fireHeart.BurningRate.Subscribe(v =>
                {
                    if (v <= 0)
                    {
                        HandleDragonBreathOff();
                        if (handMoveConvolution.Moving)
                            vfx.SetActive(true);
                    }
                });
                vfx.SetActive(false);
                dragonBreathEffect.Play();
                dragonBreathSound.Play();
                dragonBreathCollider.enabled = true;
                _fireHeart.Breathing = true;
            }
        }

        private void HandleDragonBreathOff()
        {
            _dragonSubs?.Dispose();
            _dragonSubs = null;

            dragonBreathEffect.Stop();
            dragonBreathSound.Stop();
            dragonBreathCollider.enabled = false;
            _fireHeart.Breathing = false;
        }
    }
}