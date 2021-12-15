using Kugushev.Scripts.Battle.Core.Services;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Core.Models;
using Kugushev.Scripts.Core.Services;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Core
{
    [CreateAssetMenu(menuName = Constants.AssetMenu + nameof(CoreInstaller))]
    public class CoreInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameConfigurationService>().AsSingle();
            Container.Bind<GameModeService>().AsSingle();
            Container.Bind<Score>().AsSingle();
            Container.Bind<BattleGameplayManager>().FromInstance(BattleGameplayManager.Instance).AsSingle();
        }
    }
}