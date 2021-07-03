using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Common.Core.ZenjectUtils
{
    public abstract class InPlacePrefabFactory<TContract> : MonoBehaviour, IFactory<TContract>
        where TContract : Component
    {
        [SerializeField] private GameObject prefab = default!;

        [Inject] private DiContainer _container = default!;

        TContract IFactory<TContract>.Create() =>
            _container.InstantiatePrefabForComponent<TContract>(prefab, transform);
    }

    public abstract class InPlacePrefabFactory<TParameter, TContract> : MonoBehaviour, IFactory<TParameter, TContract>
        where TContract : Component
        where TParameter : class
    {
        [SerializeField] private GameObject prefab = default!;

        [Inject] private DiContainer _container = default!;

        public TContract Create(TParameter param) =>
            _container.InstantiatePrefabForComponent<TContract>(prefab, transform, new[] {param});
    }
}