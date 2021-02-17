using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Missions.Entities;
using UnityEngine;

namespace Kugushev.Scripts.Game.Missions.Presets
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Sun Preset")]
    public class SunPreset : ScriptableObject
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private float size;

        public Sun ToSun() => new Sun(new Position(position), size);
    }
}