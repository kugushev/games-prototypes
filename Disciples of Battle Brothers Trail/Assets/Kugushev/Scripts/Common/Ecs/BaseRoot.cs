using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using UnityEngine;

namespace Kugushev.Scripts.Common.Ecs
{
    public abstract class BaseRoot : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        protected abstract void InitSystems(EcsSystems ecsSystems);
        protected abstract void Inject(EcsSystems ecsSystems);

        protected void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

#if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
            EcsSystemsObserver.Create(_systems);
#endif

            Inject(_systems);
            InitSystems(_systems);

            _systems.Init();
        }

        // {
        //     var systems = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
        //         t.IsClass &&
        //         t.GetInterfaces().Any(i => i == typeof(IEcsSystem)) &&
        //         t.HasAttribute<TSystemsAttribute>());
        //
        //     foreach (var system in systems)
        //     {
        //         var instance = (IEcsSystem) Activator.CreateInstance(system);
        //         _systems.Add(instance);
        //     }
        // }

        protected void Update()
        {
            _systems.Run();
        }

        protected void OnDestroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}