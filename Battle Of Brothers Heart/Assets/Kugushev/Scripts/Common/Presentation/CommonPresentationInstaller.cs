using Kugushev.Scripts.Common.Core.Controllers;
using Zenject;

namespace Kugushev.Scripts.Common.Presentation
{
    public class CommonPresentationInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<InputController>().AsSingle();
        }
    }
}