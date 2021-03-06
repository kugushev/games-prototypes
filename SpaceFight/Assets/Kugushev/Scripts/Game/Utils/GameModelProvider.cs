using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Utils
{
    [CreateAssetMenu(menuName = GameConstants.MenuPrefix + nameof(GameModelProvider))]
    internal class GameModelProvider : ModelProvider<GameModel, GameManager>
    {
    }
}