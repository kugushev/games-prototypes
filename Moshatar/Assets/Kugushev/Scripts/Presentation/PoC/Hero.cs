using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Image damageOverlay;
        [SerializeField] private AudioSource damageSound;

        public Vector3 HeadPosition => head.position;

        public void Suffer()
        {
            var color = damageOverlay.color;
            color.a = 0.5f;
            damageOverlay.color = color;

            damageOverlay.DOFade(0, 1f);
            damageSound.Play();
        }
    }
}