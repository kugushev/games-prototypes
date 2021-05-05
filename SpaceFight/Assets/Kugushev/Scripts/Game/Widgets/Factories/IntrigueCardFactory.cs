using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Widgets.Factories
{
    public class IntrigueCardFactory : MonoBehaviour,
        IFactory<IntrigueCardPresentationModel>
    {
        [SerializeField] private GameObject prefab = default!;

        [Inject] private DiContainer _container = default!;

        IntrigueCardPresentationModel IFactory<IntrigueCardPresentationModel>.Create()
        {
            // var args = ListPool<object>.Instance.Spawn();
            // args.Add(param1);
            // args.Add(param2);

            var instance = _container.InstantiatePrefabForComponent<IntrigueCardPresentationModel>(
                prefab, transform);

            // ListPool<object>.Instance.Despawn(args);

            return instance;
        }
    }
}