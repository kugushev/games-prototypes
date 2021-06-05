using Kugushev.Scripts.Presentation.Battle.Controllers;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle
{
    public class BattlePresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SquadController>().FromComponentInHierarchy().AsSingle();
        }
    }
}