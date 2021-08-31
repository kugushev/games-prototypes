using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class HandMoveConvolution : MonoBehaviour
    {
        public delegate void OnMoveFinished(HandMoveInfo startInfo, HandMoveInfo finishInfo);

        private XRController _xrController;
        private Transform _headTransform;
        private Vector3 _position;
        private bool _triggerPressed;
        private HandMoveInfo _triggerPressedInfo;

        public float Velocity { get; private set; }

        public event Action<HandMoveInfo> MoveStart;
        public event OnMoveFinished MoveFinished;

        private void Awake()
        {
            _xrController = GetComponentInParent<XRController>();
            if (_xrController is null)
            {
                Debug.LogError("Can't find controller");
                return;
            }

            _headTransform = (Camera.main ?? Camera.current).transform;
        }

        private void Update()
        {
            var inputDevice = _xrController.inputDevice;
            if (inputDevice.IsPressed(InputHelpers.Button.Trigger, out var isPressed) && isPressed)
            {
                if (!_triggerPressed)
                    OnTriggerPressed();
            }
            else
            {
                if (_triggerPressed)
                    OnTriggerReleased();
            }
        }


        private void OnTriggerPressed()
        {
            _triggerPressed = true;
            _triggerPressedInfo = new HandMoveInfo(_xrController.transform.position, DateTime.Now);

            MoveStart?.Invoke(_triggerPressedInfo);
        }

        private void OnTriggerReleased()
        {
            MoveFinished?.Invoke(_triggerPressedInfo, new HandMoveInfo(_xrController.transform.position, DateTime.Now));

            _triggerPressed = false;
            _triggerPressedInfo = default;
        }

        protected void FixedUpdate()
        {
            var nextPosition = transform.position;
            Velocity = (nextPosition - _position).magnitude;
            _position = nextPosition;
        }
    }
}