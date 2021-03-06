using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Entities;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Presets
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Sun Preset")]
    public class SunPreset : ScriptableObject
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private float size;

        public Sun ToSun() => new Sun(new Position(position), size);
    }
}