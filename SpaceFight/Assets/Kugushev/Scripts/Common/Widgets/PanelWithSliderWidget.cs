using Kugushev.Scripts.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Common.Widgets
{
    public class PanelWithSliderWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI seedValueText;
        [SerializeField] private Slider seedSlider;
        [SerializeField] private Slider.SliderEvent onSlide;

        public float Value
        {
            set
            {
                seedSlider.value = value;
                seedValueText.text = GetSliderText(value);
            }
        }

        public void OnSlide(float sliderValue)
        {
            seedValueText.text = GetSliderText(sliderValue);
            onSlide?.Invoke(sliderValue);
        }

        private static string GetSliderText(float sliderValue)
        {
            var integer = Mathf.FloorToInt(sliderValue);
            return StringBag.FromInt(integer);
        }
    }
}