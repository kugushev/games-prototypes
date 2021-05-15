using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MissionDataInitializer>().AsSingle();
        }
    }
}