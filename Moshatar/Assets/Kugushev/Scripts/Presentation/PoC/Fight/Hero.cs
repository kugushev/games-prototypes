using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.Presentation.PoC.Fight
{
    public class Hero : MonoBehaviour
    {
        private const int UnifiedDamage = 1;
        
        [SerializeField] private Transform head;
        [SerializeField] private Image damageOverlay;
        [SerializeField] private AudioSource damageSound;

        [Inject] private HeroStats _heroStats;

        public Vector3 HeadPosition => head.position;

        private void Awake()
        {
            _heroStats.HP.Subscribe(AdjustDamageOverlay);
        }

        public void Suffer()
        {
            damageSound.Play();
            _heroStats.HP.Value -= UnifiedDamage;
        }

        private void AdjustDamageOverlay(int hp)
        {
            float lostHp = HeroStats.MaxHP - hp;
            float norm = lostHp / HeroStats.MaxHP;
            
            var color = damageOverlay.color;
            color.a = norm;
            damageOverlay.color = color;   
        }
    }
}