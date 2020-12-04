using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts
{
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private XRController xrController;

        private void FixedUpdate()
        {
            if (xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
                    Mathf.Infinity))
                {
                    agent.destination = hit.point;
                }
            }

            if (xrController.inputDevice.IsPressed(InputHelpers.Button.Grip, out bool grip) && grip)
            {
                agent.enabled = false;
            }
            else
            {
                agent.enabled = true;
            }
        }
    }
}