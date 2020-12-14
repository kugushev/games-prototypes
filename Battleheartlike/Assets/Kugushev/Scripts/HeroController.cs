using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts
{
    [RequireComponent(typeof(XRController))]
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private UnitController unit;
        private XRController _xrController;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }

        private void FixedUpdate()
        {
            if (_xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
                    Mathf.Infinity))
                    unit.SetDestination(hit.point);
            }

            if (_xrController.inputDevice.IsPressed(InputHelpers.Button.PrimaryButton, out bool button) && button)
            {
                unit.Attack();
            }
        }
    }
}