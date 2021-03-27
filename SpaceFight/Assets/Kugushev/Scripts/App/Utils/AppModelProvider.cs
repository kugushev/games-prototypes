using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.Models;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.App.Utils
{
    [CreateAssetMenu(menuName = AppConstants.MenuPrefix + nameof(AppModelProvider))]
    internal class AppModelProvider : ModelProvider<AppModel>
    {
    }
}