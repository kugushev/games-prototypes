using System;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class HeroUnitPresenter : MonoBehaviour
    {
        private HeroFighter _model;

        public void Init(HeroFighter model)
        {
            _model = model;
            OnModelSet(_model);
        }

        // todo: handle hit (head collider)
        // todo: handle death
        // todo: hp add regen

        private void Update()
        {
            var p = transform.position;

            var v = new Vector2(p.x, p.z); // as intended - just translate 2D to 3D
            _model.UpdatePosition(new Position(v));
        }

        private void OnModelSet(HeroFighter model)
        {
            model.Hurt += OnHurt;
            model.Die += OnDie;
        }

        private void OnHurt()
        {
            
        }

        private void OnDie()
        {
            
        }
    }
}