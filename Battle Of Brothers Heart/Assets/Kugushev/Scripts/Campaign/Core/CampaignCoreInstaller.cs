using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Kugushev.Scripts.Campaign.Core.Services;
using Kugushev.Scripts.Campaign.Core.ValueObjects.Orders;
using Kugushev.Scripts.Game.Core.Models;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();

            Container.BindInterfacesAndSelfTo<Wayfarers>().AsSingle();

            Container.BindFactory<City, OrderVisitCity, OrderVisitCity.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<BanditWayfarer, OrderAttackBandit, OrderAttackBandit.Factory>()
                .FromPoolableMemoryPool();
        }
    }
}