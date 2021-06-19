using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<World>().AsSingle();
            Container.BindInterfacesAndSelfTo<WayfarersManager>().AsSingle();
        }
    }
}