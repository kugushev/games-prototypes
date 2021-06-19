using Cinemachine;
using UnityEngine;

namespace Kugushev.Scripts.Common.Presentation.Utils
{
    public class OnScrollZoomer : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cam = default!;
        [SerializeField] private float mouseZoomSpeed;
        [SerializeField] private float zoomMinBound;
        [SerializeField] private float zoomMaxBound;

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(scroll, mouseZoomSpeed);
        }


        void Zoom(float deltaMagnitudeDiff, float speed)
        {
            cam.m_Lens.OrthographicSize -= deltaMagnitudeDiff * speed;
            cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, zoomMinBound, zoomMaxBound);
        }
    }
}