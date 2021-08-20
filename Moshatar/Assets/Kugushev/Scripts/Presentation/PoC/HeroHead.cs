using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    [RequireComponent(typeof(VelocityMeasured))]
    public class HeroHead : MonoBehaviour
    {
        private const float ChargeVelocity = 0.1f;
        private const float ChargingSwing = 0.1f;

        [Inject] private Hero _hero;

        private VelocityMeasured _velocityMeasured;
        private bool _charging;
        private Vector2 _chargeStartPosition;

        private void Awake()
        {
            _velocityMeasured = GetComponent<VelocityMeasured>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyProjectile"))
            {
                _hero.Suffer();
            }
        }

        private void Update()
        {
            if (_charging)
                return;

            if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            {
                _charging = true;
                _chargeStartPosition = GetSurfacePosition();
                print($"Charging at {_chargeStartPosition}");
            }

            // else
            // {
            //     _charging = false;
            //     _chargeStartPosition = default;
            // }
        }

        private void FixedUpdate()
        {
            if (_charging)
            {
                print($"Velocity {_velocityMeasured.Velocity}");
                if (_velocityMeasured.Velocity >= ChargeVelocity)
                {
                    var chargeStopPosition = GetSurfacePosition();
                    var distance = Vector3.Distance(_chargeStartPosition, chargeStopPosition);
                    print($"Distance {distance}");
                    if (distance >= ChargingSwing)
                    {
                        print("Charge");
                        Debug.DrawLine(
                            new Vector3(_chargeStartPosition.x, 0, _chargeStartPosition.y),
                            new Vector3(chargeStopPosition.x, 0, chargeStopPosition.y),
                            Color.magenta, 60);

                        _charging = false;
                        _chargeStartPosition = default;
                    }
                }
            }
        }

        private Vector2 GetSurfacePosition()
        {
            var position = transform.position;
            return new Vector2(position.x, position.z);
        }
    }
}