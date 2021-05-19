using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Core.ContextManagement;
using Kugushev.Scripts.Mission.Core.ContextManagement.Transitions;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Core.Services;
using Kugushev.Scripts.Mission.Core.Specifications;
using Kugushev.Scripts.Mission.Core.ValueObjects;
using Kugushev.Scripts.Mission.Enums;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Mission.Core
{
    public class MissionInstaller : MonoInstaller
    {
        [SerializeField] private PerksRegistry perksRegistry;

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

            Container.Bind<OrderBuilder>().WithId(GameplayConstants.LeftHandCommander).AsSingle();
            Container.Bind<OrderBuilder>().WithId(GameplayConstants.RightHandCommander).AsSingle();
        }

        private void InstallContextManagement()
        {
            Container.Bind<BriefingState>().AsSingle();
            Container.Bind<ExecutionState>().AsSingle();

            Container.Bind<ToExecutionTransition>().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<PerksRegistry>().FromScriptableObject(perksRegistry).AsSingle();

            Container.Bind<EffectsService>().AsSingle();
            Container.Bind<PlanetarySystemGenerationService>().AsSingle();
            Container.Bind<PerksSearchService>().AsSingle();
            Container.Bind<EventsCollectingService>().AsSingle();
        }

        private void InstallSpecs()
        {
            Container.Bind<PlanetarySystemSpecs>().AsSingle();
            Container.Bind<PlanetSpecsRegistry>().AsSingle();
        }
    }
}