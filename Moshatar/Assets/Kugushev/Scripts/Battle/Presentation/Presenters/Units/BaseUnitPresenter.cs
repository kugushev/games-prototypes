using System;
using Kugushev.Scripts.Battle.Core.Enums;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public abstract class BaseUnitPresenter : MonoBehaviour
    {
        [SerializeField] private SimpleHealthBar simpleHealthBar = default!;

        protected Animator Animator;

        public abstract BaseFighter Model { get; }

        protected void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        protected void Start()
        {
            Model.Character.HP.Subscribe(OnHitPointsChanged).AddTo(this);
            Model.Position.Subscribe(OnPositionChanged).AddTo(this);
            Model.Activity.Subscribe(OnActivityChanged).AddTo(this);
            //
            Model.Attacking += OnAttacking;
            // _model.AttackCanceled += OnAttackCanceled;
            Model.Hurt += OnHurt;
            Model.Die += OnDie;

            OnStart();
        }
        
        protected virtual void OnStart(){}

        protected void OnDestroy()
        {
            Model.Attacking -= OnAttacking;
            // _model.AttackCanceled -= OnAttackCanceled;
            Model.Hurt -= OnHurt;
            Model.Die -= OnDie;
        }

        private void OnHitPointsChanged(int hitPoints)
        {
            if (simpleHealthBar is {}) 
                simpleHealthBar.UpdateBar(hitPoints, Model.Character.MaxHP);
        }

        private void OnPositionChanged(Position newPosition)
        {
            var t = transform;

            Vector3 vector = t.position;

            vector.x = newPosition.Vector.x;
            vector.z = newPosition.Vector.y; // as intended - just translate 2D to 3D

            t.LookAt(vector);
            t.position = vector;
        }

        protected abstract void OnActivityChanged(ActivityType newActivityType);

        protected abstract void OnAttacking();

        protected abstract void OnHurt();

        protected abstract void OnDie();
    }
}