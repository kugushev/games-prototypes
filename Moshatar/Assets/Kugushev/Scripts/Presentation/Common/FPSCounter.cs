using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Common
{
    public class FPSCounter: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Update()
        {
            float fps = 1f / Time.unscaledDeltaTime;
            int fpsInt = Mathf.FloorToInt(fps);
            text.text = StringBag.FromInt(fpsInt);
        }
    }
}