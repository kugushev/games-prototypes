using System;
using Kugushev.Scripts.Core.Battle;
using Kugushev.Scripts.Core.Battle.Enums;
using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using UnityEngine;
using Zenject;
using UniRx;

namespace Kugushev.Scripts.Presentation.Battle.Presenters
{
    public class SquadBaseUnitPresenter : BaseUnitPresenter
    {
        [SerializeField] private SpriteRenderer selectionMarker = default!;

        [Inject] private SquadController _squadController = default!;

        private bool _selected;

        public SquadUnit Unit { get; } = new SquadUnit();

        protected override BaseUnit Model => Unit;

        protected override void OnAwake()
        {
            _squadController.Register(this);
        }

        protected override void OnDestruction()
        {
            _squadController.Unregister(this);
        }

        public void Select()
        {
            if (Input.GetMouseButton(0))
                if (!_selected)
                {
                    _selected = true;
                    selectionMarker.enabled = true;
                    _squadController.UnitSelected(this);
                }
        }

        public void Deselect()
        {
            if (_selected)
            {
                _selected = false;
                selectionMarker.enabled = false;
            }
        }
    }
}