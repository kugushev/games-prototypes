using System;
using Kugushev.Scripts.Presentation.PoC.Common;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class FireOrbController : MonoBehaviour
    {
        private const float MinVelocity = 0.06f;
        private const float ProjectileSpeed = 10f;

        [SerializeField] private HandMoveConvolution handMoveConvolution;
        [SerializeField] private GameObject vfx;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem frontEffect;
        [SerializeField] private AudioSource frontEffectSound;


        [Inject] private HeroHeadController _heroHeadController;
        [Inject] private Projectile.Factory _playerProjectile;

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

        private void HandMoveConvolutionOnMoveStart(HandMoveInfo startInfo)
        {
            vfx.SetActive(true);
            audioSource.Play();
        }

        private void HandMoveConvolutionOnMoveFinished(HandMoveInfo startinfo, HandMoveInfo finishinfo)
        {
            vfx.SetActive(false);
            audioSource.Stop();

            Recognize(startinfo, finishinfo);
        }

        private void Recognize(HandMoveInfo startinfo, HandMoveInfo finishinfo)
        {
            // var headPosition = _heroHeadController.Position;

            // var expectedStartPoint = new Vector3(headPosition.x, startinfo.Position.y, headPosition.z);

            // var expectedVector = finishinfo.Position - expectedStartPoint;
            var actualVector = finishinfo.Position - startinfo.Position;

            // Debug.DrawLine(expectedStartPoint, finishinfo.Position, Color.green, 15f);
            // Debug.DrawLine(startinfo.Position, finishinfo.Position, Color.red, 15f);

            if (handMoveConvolution.Velocity > MinVelocity)
                _playerProjectile.Create(finishinfo.Position, actualVector, ProjectileSpeed);
            else
                print($"No enough velocity: {handMoveConvolution.Velocity}");
            
            // var cos = Vector3.Dot(expectedVector.normalized, actualVector.normalized);

            // const float cos45Deg = 0.7f;
            // if (cos >= cos45Deg)
            // {
            //     // todo: add velocity check
            //     // var deltaTime = Convert.ToSingle((finishinfo.Time - startinfo.Time).TotalMilliseconds);
            //     // var deltaDistance = actualVector.magnitude;
            //     //
            //     // var avgHandSpeed = deltaDistance / deltaTime;
            //
            //     if (handMoveConvolution.Velocity > MinVelocity)
            //         _playerProjectile.Create(finishinfo.Position, actualVector, ProjectileSpeed);
            //     else
            //         print($"No enough velocity: {handMoveConvolution.Velocity}");
            // }
            // else
            // {
            //     //if (handMoveConvolution.Velocity > 0)
            //     {
            //         var spawnPoint = (finishinfo.Position + startinfo.Position) / 2;
            //
            //         var expectedHeart = new Vector3(headPosition.x, headPosition.y * 0.8f, headPosition.z);
            //         var direction = (spawnPoint - expectedHeart).normalized;
            //         var euler = new Vector3(
            //             Mathf.Acos(direction.x) * Mathf.Rad2Deg,
            //             Mathf.Acos(direction.y) * Mathf.Rad2Deg - 90f,
            //             Mathf.Acos(direction.z) * Mathf.Rad2Deg
            //         );
            //
            //         frontEffect.Stop();
            //         frontEffect.transform.position = spawnPoint;
            //         frontEffect.transform.rotation = Quaternion.Euler(euler);
            //         // frontEffect.transform.LookAt(finishinfo.Position + startinfo.Position);
            //         frontEffect.Play();
            //
            //         frontEffectSound.Stop();
            //         frontEffectSound.Play();
            //     }
            // }
        }
    }
}