using System;
using System.Linq;
using Kugushev.Scripts.Core.Models;
using Kugushev.Scripts.Core.Services;
using Kugushev.Scripts.Presentation.PoC.Fight;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Music
{
    public class GameDirector : MonoBehaviour
    {
        private const float BitDelta = 0.15f;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SongConfiguration songConfiguration;
        [SerializeField] private Light mainLight;
        [SerializeField] private HeroStats heroStats;

        [Inject] private Score _score;
        [Inject] private GameModeService _gameModeService;

        public SongSectionType CurrentSectionType { get; private set; }
        public bool IsBit { get; private set; }

        private void Awake()
        {
            audioSource.clip = songConfiguration.Clip;
            audioSource.Play();

            CurrentSectionType = songConfiguration.Sections.First().Type;
        }

        private void Update()
        {
            if (!audioSource.isPlaying || heroStats.HP.Value < 0)
            {
                _score.Register(heroStats.Gold.Value);
                _gameModeService.BackToMenu();
                return;
            }

            SongSection section = null;
            foreach (var s in songConfiguration.Sections)
            {
                if (s.Offset.TotalSeconds > audioSource.time)
                    break;
                section = s;
            }

            if (section == null)
                return;

            CurrentSectionType = section.Type;

            switch (section.Type)
            {
                case SongSectionType.Pause:
                    break;
                case SongSectionType.Menace:
                    break;
                case SongSectionType.Battle:
                    HandleBattle(section);
                    break;
                case SongSectionType.Slam:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleBattle(SongSection section)
        {
            var mod = audioSource.time % section.Pace;
            IsBit = mod < BitDelta;
            
            if (mainLight is null)
                return;
            mainLight.color = IsBit ? Color.white : Color.gray;
        }
    }
}