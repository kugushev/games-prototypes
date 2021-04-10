using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class HandWidget : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Transform indexPoint;

        public void UpdateSlider(float value) => slider.value = value;

        public Vector3 IndexPointPosition => indexPoint.position;
    }
}