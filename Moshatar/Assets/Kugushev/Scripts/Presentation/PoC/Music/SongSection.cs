using System;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Music
{
    [Serializable]
    public class SongSection
    {
        [SerializeField] private SongSectionType type;
        [SerializeField] private float pace;
        [Header("Offset")] [SerializeField] private int minutes;
        [SerializeField] private int seconds;
        [SerializeField] private int milliseconds;

        public SongSectionType Type => type;
        public float Pace => pace;
        public TimeSpan Offset => new TimeSpan(0, 0, minutes, seconds, milliseconds);
    }
}