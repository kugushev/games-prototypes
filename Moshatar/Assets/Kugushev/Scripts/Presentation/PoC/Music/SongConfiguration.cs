using System.Collections.Generic;
using Kugushev.Scripts.Common;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Music
{
    [CreateAssetMenu(menuName = Constants.AssetMenu + nameof(SongConfiguration))]
    public class SongConfiguration : ScriptableObject
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private SongSection[] sections;

        public AudioClip Clip => clip;

        public IReadOnlyList<SongSection> Sections => sections;
    }
}