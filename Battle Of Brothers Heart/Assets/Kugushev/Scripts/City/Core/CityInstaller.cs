using Kugushev.Scripts.City.Core.Models;
using Kugushev.Scripts.City.Core.Models.Villagers;
using Kugushev.Scripts.City.Core.Services;
using Kugushev.Scripts.City.Core.ValueObjects.Orders;
using Zenject;

namespace Kugushev.Scripts.City.Core
{
    public class CityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerVillager>().AsSingle();
            Container.Bind<RoadSign>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();

            Container.BindFactory<RoadSign, OrderGoToRoadSign, OrderGoToRoadSign.Factory>().FromPoolableMemoryPool();
        }
    }
}