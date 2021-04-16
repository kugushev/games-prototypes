using System;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class HandWidget : MonoBehaviour
    {
        [SerializeField] private Canvas? canvas;
        [SerializeField] private Slider? slider;
        [SerializeField] private Transform? indexPoint;

        public event Action? SurrenderClick;
        public void Setup(Camera mainCamera)
        {
            Asserting.NotNull(canvas);
            canvas.worldCamera = mainCamera;
        }

        public void UpdateSlider(float value)
        {
            Asserting.NotNull(slider);
            slider.value = value;
        }

        public Vector3 IndexPointPosition
        {
            get
            {
                Asserting.NotNull(indexPoint);
                return indexPoint.position;
            }
        }

        public void OnButtonSurrenderClick() => SurrenderClick?.Invoke();
    }
}