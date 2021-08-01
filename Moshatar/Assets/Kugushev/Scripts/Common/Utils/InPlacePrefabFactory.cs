using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Common.Utils
{
    public abstract class InPlacePrefabFactory<T> : MonoBehaviour, IFactory<T>
        where T : Component
    {
        [SerializeField] private GameObject prefab = default!;

        [Inject] private DiContainer _container = default!;

        T IFactory<T>.Create() => _container.InstantiatePrefabForComponent<T>(prefab, transform);
    }
}