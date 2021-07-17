using UnityEngine;

namespace Kugushev.Scripts.Common.Presentation.Utils
{
    public class CinemachineAnchor : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera = default!;

        void Update()
        {
            var pos = mainCamera.ScreenToViewportPoint(Input.mousePosition);

            CheckBorders(ref pos.x);
            CheckBorders(ref pos.y);

            var world = mainCamera.ViewportToWorldPoint(pos);
            world.z = 0; // to avoid issues with loosing view point
            transform.position = world;
        }

        private static void CheckBorders(ref float dimension)
        {
            if (dimension < 0)
                dimension = 0;
            else if (dimension > 1)
                dimension = 1;
        }
    }
}