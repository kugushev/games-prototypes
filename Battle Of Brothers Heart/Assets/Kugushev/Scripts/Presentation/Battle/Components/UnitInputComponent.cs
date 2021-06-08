using Kugushev.Scripts.Presentation.Battle.Controllers;
using Kugushev.Scripts.Presentation.Battle.Presenters;
using Kugushev.Scripts.Presentation.Battle.Presenters.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Components
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