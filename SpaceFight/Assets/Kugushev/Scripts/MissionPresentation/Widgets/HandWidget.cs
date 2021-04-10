using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.MissionPresentation.Widgets
{
    public class HandWidget : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Slider slider;
        [SerializeField] private Transform indexPoint;

        public event Action SurrenderClick;
        public void Setup(Camera mainCamera) => canvas.worldCamera = mainCamera;
        public void UpdateSlider(float value) => slider.value = value;
        public Vector3 IndexPointPosition => indexPoint.position;

        public void OnButtonSurrenderClick() => SurrenderClick?.Invoke();
    }
}