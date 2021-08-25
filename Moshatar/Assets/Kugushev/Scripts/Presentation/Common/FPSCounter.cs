using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Common
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private float _frameCount;
        private float _dt;
        private float _fps;
        private float updateRate = 4.0f;

        private void Update()
        {
            _frameCount++;
            _dt += Time.deltaTime;
            if (_dt > 1.0f / updateRate)
            {
                _fps = _frameCount / _dt;
                _frameCount = 0;
                _dt -= 1.0f / updateRate;
                int fpsInt = Mathf.FloorToInt(_fps);
                text.text = StringBag.FromInt(fpsInt);
            }

            // float fps = 1f / Time.unscaledDeltaTime;
            // int fpsInt = Mathf.FloorToInt(fps);
            // text.text = StringBag.FromInt(fpsInt);
        }
    }
}