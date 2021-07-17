using System;
using System.Linq;
using System.Reflection;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Factories;
using Kugushev.Scripts.Factories.Abstractions;
using Kugushev.Scripts.Systems;
using Kugushev.Scripts.Systems.AI;
using Kugushev.Scripts.Views;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using Unity.VisualScripting;
using UnityEngine;

namespace Kugushev.Scripts
{
    public class CampaignRoot : MonoBehaviour
    {
        [SerializeField] private WorldView worldView;
        [SerializeField] private BaseEntitiesFactory[] entitiesFactories;

        private EcsWorld _world;
        private EcsSystems _systems;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

#if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
            EcsSystemsObserver.Create(_systems);
#endif

            _systems.Inject(worldView);
            InitCampaignSystems();
            InitFactories();

            _systems.Init();
        }

        private void InitFactories()
        {
            foreach (var factory in entitiesFactories)
            {
                _systems.Inject(factory);
            }
        }

        private void InitCampaignSystems()
        {
            var systems = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                t.IsClass &&
                t.GetInterfaces().Any(i => i == typeof(IEcsSystem)) &&
                t.HasAttribute<CampaignSystemAttribute>());

            foreach (var system in systems)
            {
                var instance = (IEcsSystem) Activator.CreateInstance(system);
                _systems.Add(instance);
            }
        }

        void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}