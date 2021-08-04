using UnityEngine;
using Leopotam.Ecs;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;

#endif

namespace Kugushev.Scripts.Core
{
    public class EcsRoot : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        private void InitSystems(EcsSystems ecsSystems)
        {
        }

        private void Inject(EcsSystems ecsSystems)
        {
        }

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