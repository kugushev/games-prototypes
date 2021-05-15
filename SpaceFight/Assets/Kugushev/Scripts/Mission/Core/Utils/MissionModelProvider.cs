using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Models;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Utils
{
    [CreateAssetMenu(menuName = MissionConstants.MenuPrefix + nameof(MissionModelProvider))]
    public class MissionModelProvider : ModelProvider<MissionModel>
    {
    }
}