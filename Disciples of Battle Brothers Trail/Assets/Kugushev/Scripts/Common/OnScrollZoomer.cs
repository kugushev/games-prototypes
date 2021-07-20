using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kugushev.Scripts.Common
{
    public class OnScrollZoomer : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cam = default!;
        [SerializeField] private float mouseZoomSpeed;
        [SerializeField] private float zoomMinBound;
        [SerializeField] private float zoomMaxBound;

        private void Update()
        {
            if (Mouse.current != null)
            {
                float scroll = Mouse.current.scroll.y.ReadValue();
                if (scroll != 0f) 
                    Zoom(scroll, mouseZoomSpeed);
            }
        }

        void Zoom(float deltaMagnitudeDiff, float speed)
        {
            cam.m_Lens.OrthographicSize -= deltaMagnitudeDiff * speed;
            cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, zoomMinBound, zoomMaxBound);
        }
    }
}