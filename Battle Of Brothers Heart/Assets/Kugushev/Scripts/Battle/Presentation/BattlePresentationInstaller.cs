using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Presentation.Controllers;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation
{
    public class BattlePresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IInputController>().To<InputController>().FromResolve();
        }
    }
}