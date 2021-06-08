using System;
using Kugushev.Scripts.Core.Battle.Models.Units;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Units
{
    public class PlayerUnitPresenter : BaseUnitPresenter
    {
        [SerializeField] private SpriteRenderer selectionMarker = default!;

        public PlayerUnit Unit { get; private set; }

        public override BaseUnit Model => Unit;

        public void Init(PlayerUnit playerUnit) => Unit = playerUnit;

        protected override void OnStart()
        {
            Unit.Selected.Subscribe(OnSelectionChanged).AddTo(this);
        }

        private void OnSelectionChanged(bool selected) => selectionMarker.enabled = selected;
    }
}