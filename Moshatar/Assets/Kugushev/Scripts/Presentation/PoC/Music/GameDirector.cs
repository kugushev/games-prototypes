using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Kugushev.Scripts.Presentation.PoC.Music
{
    public class GameDirector : MonoBehaviour
    {
        private const float BitDelta = 0.15f;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SongConfiguration songConfiguration;
        [SerializeField] private Light mainLight;

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
            mainLight.enabled = IsBit;
        }
    }
}