using Kugushev.Scripts.Campaign.Core.Models;
using Zenject;

namespace Kugushev.Scripts.Campaign.Core
{
    public class CampaignCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<World>().AsSingle();
        }
    }
}