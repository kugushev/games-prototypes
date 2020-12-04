using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kugushev.Scripts
{
    public class UnitController: MonoBehaviour
    {
        public void OnHover(XRBaseInteractor interactor)
        {
            transform.Rotate(0, Mathf.PI / 2f, 0);
        }
    }
}