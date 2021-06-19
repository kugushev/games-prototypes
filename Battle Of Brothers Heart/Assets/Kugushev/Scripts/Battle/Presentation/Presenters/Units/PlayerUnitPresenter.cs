using Kugushev.Scripts.Battle.Core.Models.Units;
using Kugushev.Scripts.Common.Core.Exceptions;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class PlayerUnitPresenter : BaseUnitPresenter
    {
        [SerializeField] private SpriteRenderer selectionMarker = default!;

        private PlayerUnit? _model;
        public override BaseUnit Model => _model ?? throw new PropertyIsNotInitializedException(nameof(Model));

        public void Init(PlayerUnit model) => _model = model;

        protected override void OnStart()
        {
            _model?.Selected.Subscribe(OnSelectionChanged).AddTo(this);
        }

        private void OnSelectionChanged(bool selected) => selectionMarker.enabled = selected;
    }
}