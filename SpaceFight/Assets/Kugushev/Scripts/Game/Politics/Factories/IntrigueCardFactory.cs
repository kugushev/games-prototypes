using Kugushev.Scripts.Game.Politics.PresentationModels;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.Factories
{
    public class IntrigueCardFactory : MonoBehaviour,
        IFactory<IntrigueCardPresentationModel>
    {
        [SerializeField] private GameObject prefab = default!;

        [Inject] private DiContainer _container = default!;

        IntrigueCardPresentationModel IFactory<IntrigueCardPresentationModel>.Create() =>
            _container.InstantiatePrefabForComponent<IntrigueCardPresentationModel>(
                prefab, transform);
    }
}