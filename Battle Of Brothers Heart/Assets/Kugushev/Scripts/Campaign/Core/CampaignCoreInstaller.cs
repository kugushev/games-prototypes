using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Campaign.Core.Services;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<World>().AsSingle();
            Container.BindInterfacesAndSelfTo<WayfarersManager>().AsSingle();
            Container.BindFactory<City, OrderVisitCity, OrderVisitCity.Factory>().FromPoolableMemoryPool();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }
    }
}