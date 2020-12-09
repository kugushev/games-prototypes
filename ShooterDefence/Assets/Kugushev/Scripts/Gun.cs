using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Kugushev.Scripts
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform launchPoint;
        [SerializeField] private FightColors fightColor;
        [SerializeField] private GameObject decorRed;
        [SerializeField] private GameObject decorGreen;
        [SerializeField] private GameObject decorBlue;
        [SerializeField] private AudioSource shootSound;
        [SerializeField] private bool isShooting;
        private GameController _gameController;
        private FightColors? fightColorOverride;

        private void Awake()
        {
            var obj = GameObject.FindWithTag("GameController");
            _gameController = obj.GetComponent<GameController>();
        }

        private void Start()
        {
            switch (fightColor)
            {
                case FightColors.Red:
                    decorRed.SetActive(true);
                    break;
                case FightColors.Green:
                    decorGreen.SetActive(true);
                    break;
                case FightColors.Blue:
                    decorBlue.SetActive(true);
                    break;
                case FightColors.Yellow:
                    break;
                case FightColors.Cyan:
                    break;
                case FightColors.Magenta:
                    break;
                default:
                    Debug.LogError("Unexpected fightColor");
                    break;
            }
            
            StartCoroutine(Shooting());
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (fightColor)
            {
                case FightColors.Red:
                    if (other.CompareTag("GunBlue")) 
                        fightColorOverride = FightColors.Magenta;
                    if (other.CompareTag("GunGreen")) 
                        fightColorOverride = FightColors.Yellow;
                    break;
                case FightColors.Green:
                    if (other.CompareTag("GunBlue")) 
                        fightColorOverride = FightColors.Cyan;
                    if (other.CompareTag("GunRed")) 
                        fightColorOverride = FightColors.Yellow;
                    break;
                case FightColors.Blue:
                    if (other.CompareTag("GunGreen")) 
                        fightColorOverride = FightColors.Cyan;
                    if (other.CompareTag("GunRed")) 
                        fightColorOverride = FightColors.Magenta;
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            fightColorOverride = null;
        }

        public void PushTrigger() => isShooting = true;
        public void ReleaseTrigger() => isShooting = false;

        private IEnumerator Shooting()
        {
            var timeout = new WaitForSeconds(0.2f);
            while (true)
            {
                if (isShooting)
                {
                    var color = fightColorOverride ?? fightColor;
                    var bullet = _gameController.GetNextPlayerBullet(color);

                    var bulletTransform = bullet.transform;
                    bulletTransform.position = launchPoint.position;
                    bulletTransform.rotation = launchPoint.rotation;
                    
                    shootSound.Play();
                }
                
                yield return timeout;
            }
        }
    }
}