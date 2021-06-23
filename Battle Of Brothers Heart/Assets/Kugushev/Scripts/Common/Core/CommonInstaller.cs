using Kugushev.Scripts.Common.Core.Controllers;
using Zenject;

namespace Kugushev.Scripts.Common.Core
{
    public class CommonInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
        }
    }
}