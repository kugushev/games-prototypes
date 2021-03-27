using Kugushev.Scripts.App.Constants;
using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.App.Utils
{
    [CreateAssetMenu(menuName = AppConstants.MenuPrefix + nameof(GameSceneParametersPipeline))]
    public class GameSceneParametersPipeline: SceneParametersPipeline<GameInfo>
    {
        
    }
}