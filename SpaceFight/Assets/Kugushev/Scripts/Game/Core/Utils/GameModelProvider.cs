using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Core.Utils
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(GameModelProvider))]
    internal class GameModelProvider : ModelProvider<GameModel>
    {
    }
}