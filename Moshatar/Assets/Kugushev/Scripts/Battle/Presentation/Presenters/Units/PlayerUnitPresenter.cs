using Kugushev.Scripts.Battle.Core.Exceptions;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class PlayerUnitPresenter : BaseUnitPresenter
    {
        [SerializeField] private SpriteRenderer selectionMarker = default!;

        private PlayerFighter? _model;
        public override BaseFighter Model => _model ?? throw new PropertyIsNotInitializedException();

        public void Init(PlayerFighter model) => _model = model;

        protected override void OnStart()
        {
            _model?.Selected.Subscribe(OnSelectionChanged).AddTo(this);
        }

        private void OnSelectionChanged(bool selected) => selectionMarker.enabled = selected;
    }
}