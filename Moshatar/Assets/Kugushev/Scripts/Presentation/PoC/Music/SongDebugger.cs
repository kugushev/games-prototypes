using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Presentation.PoC.Music
{
    public class SongDebugger : MonoBehaviour
    {
        private const float BitDelta = 0.05f;
        private const float ZoneRange = 0.3f;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SongConfiguration songConfiguration;
        [SerializeField] private Button playPause;
        [SerializeField] private Slider playback;
        [SerializeField] private Image left;
        [SerializeField] private Image right;
        [SerializeField] private Image all;

        private bool _lastIsRight;
        
        
        private void Awake()
        {
            audioSource.clip = songConfiguration.Clip;

            playPause.onClick.AddListener(TogglePlayPause);
            playback.onValueChanged.AddListener(OnPlaybackChanged);
        }

        private void OnDestroy()
        {
            playPause.onClick.RemoveListener(TogglePlayPause);
            playback.onValueChanged.RemoveListener(OnPlaybackChanged);
        }

        private void Update()
        {
            if (audioSource.isPlaying)
            {
                playback.SetValueWithoutNotify(audioSource.time / songConfiguration.Clip.length);

                SongSection section = null;
                foreach (var s in songConfiguration.Sections)
                {
                    if (s.Offset.TotalSeconds > audioSource.time)
                        break;
                    section = s;
                }

                if (section == null)
                    return;

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
        }

        private void HandleBattle(SongSection section)
        {
            // show hit - black
            var mod = audioSource.time % section.Pace;


           // ApplyBit(all, section, mod);

           //todo: fix bug: in this case you may visit this code multiple times per bit
            if (_lastIsRight)
            {
                ApplyBit(left, section, mod);
                right.color = Color.white;
                _lastIsRight = false;
            }
            else
            {
                ApplyBit(right, section, mod);
                left.color = Color.white;
                _lastIsRight = true;
            }


            // show zone - red
        }

        private void ApplyBit(Image img, SongSection section, float mod)
        {
            //var halfRedZone = section.Pace * ZoneRange / 2;

            if (mod < BitDelta)
                img.color = Color.black;
            // else 
            // if (mod < halfRedZone)
            //     img.color = Color.red;
            // else if (mod > section.Pace - halfRedZone)
            //     img.color = Color.red;
            else
                img.color = Color.white;
        }

        private void TogglePlayPause()
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
            else
                audioSource.Play();
        }

        private void OnPlaybackChanged(float position)
        {
            audioSource.time = songConfiguration.Clip.length * position;
        }
    }
}