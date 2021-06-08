using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle
{
    public class BattlePresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IInputController>().To<InputController>().FromResolve();
            Container.Bind<SquadController>().FromComponentInHierarchy().AsSingle();
        }
    }
}