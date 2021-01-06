using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    /// <summary>
    /// Alternative input and other things for debugging in not VR mode
    /// </summary>
    public class FlatModeController : MonoBehaviour
    {
        [SerializeField] private GameObject xrRig;
        [SerializeField] private Camera flatCamera;

        private void Awake()
        {
            xrRig.SetActive(false);
        }
    }
}