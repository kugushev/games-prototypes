using Kugushev.Scripts.Common.UI;
using Zenject;

namespace Kugushev.Scripts.City
{
    public class CityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ModalMenuManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}