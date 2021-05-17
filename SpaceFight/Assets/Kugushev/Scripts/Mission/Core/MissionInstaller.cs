using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Core.ContextManagement;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MissionDataInitializer>().AsSingle();

            InstallModels();
            InstallContextManagement();
            InstallServices();
            InstallSpecs();
        }

        private void InstallModels()
        {
            Container.Bind<PlanetarySystem>().AsSingle();
            Container.Bind<IPlanetarySystem>().To<PlanetarySystem>().FromResolve();

            Container.BindFactory<Faction, PlanetSize, Production, Orbit, Power, Planet, Planet.Factory>();
        }

        private void InstallContextManagement()
        {
            Container.Bind<BriefingState>().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<PlanetarySystemGenerationService>().AsSingle();
        }

        private void InstallSpecs()
        {
            Container.Bind<PlanetarySystemSpecs>().AsSingle();
            Container.Bind<PlanetSpecsRegistry>().AsSingle();
        }
    }
}