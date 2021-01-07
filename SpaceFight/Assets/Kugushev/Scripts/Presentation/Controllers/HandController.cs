using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts.Presentation.Controllers
{
    [RequireComponent(typeof(XRController))]
    public class HandController : MonoBehaviour
    {
        private XRController _xrController;

        private void Awake()
        {
            _xrController = GetComponent<XRController>();
        }
    }
}