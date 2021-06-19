using Kugushev.Scripts.Battle.Presentation.Controllers;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation.Components
{
    [RequireComponent(typeof(BaseUnitPresenter))]
    public class UnitInputComponent : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private InputController _inputController = default!;

        private BaseUnitPresenter _baseUnitPresenter = default!;

        private void Awake()
        {
            _baseUnitPresenter = GetComponent<BaseUnitPresenter>();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _inputController.OnUnitClick(_baseUnitPresenter.Model, eventData.button);
        }
    }
}