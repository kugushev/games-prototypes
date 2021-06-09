using System;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Common.Exceptions;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Units
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