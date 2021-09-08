using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class HandMoveConvolution : MonoBehaviour
    {
        public delegate void OnMoveFinished(HandMoveInfo startInfo, HandMoveInfo finishInfo);

        private XRController _xrController;
        private Vector3 _position;
        private bool _triggerPressed;
        private HandMoveInfo _triggerPressedInfo;

        public float Velocity { get; private set; }

        public event Action<HandMoveInfo> MoveStart;
        public event OnMoveFinished MoveFinished;
        public bool Moving => _triggerPressed;

        private void Awake()
        {
            _xrController = GetComponentInParent<XRController>();
            if (_xrController is null)
            {
                Debug.LogError("Can't find controller");
                return;
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                if (_triggerPressed)
                    OnTriggerReleased();
                else
                    OnTriggerPressed();
            }

#else
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

#endif
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
            var newVelocity = (nextPosition - _position).magnitude;
            if (newVelocity > 0f)
                Velocity = newVelocity;
            _position = nextPosition;
        }
    }
}