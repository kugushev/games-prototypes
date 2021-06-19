using Kugushev.Scripts.Game.Core.Models.AI;
using Zenject;

namespace Kugushev.Scripts.Game.Core
{
    public class GameCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AgentsManager>().AsSingle();
        }
    }
}