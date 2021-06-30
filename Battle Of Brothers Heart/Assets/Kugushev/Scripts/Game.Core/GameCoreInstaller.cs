using Kugushev.Scripts.Common.Core;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Common.Core.AI.Orders;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models;
using Zenject;

namespace Kugushev.Scripts.Game.Core
{
    public class GameCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<CommonInstaller>();

            Container.Bind<GameModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldUnitsManager>().AsSingle();
            Container.Bind<BattleManager>().AsSingle();
        }
    }
}