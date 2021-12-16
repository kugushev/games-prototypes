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
        [SerializeField] private SimpleHealthBar simpleHealthBar;
        [SerializeField] private GameObject simpleHealthBarRoot;

        protected Animator Animator;
        protected Vector3 DefaultScale;

        protected void Awake()
        {
            Animator = GetComponent<Animator>();
            DefaultScale = transform.localScale;
        }

        protected void OnModelSet(BaseFighter model)
        {
            model.Character.HP.Subscribe(i => OnHitPointsChanged(i, model.Character.MaxHP)).AddTo(this);
            model.Position.Subscribe(OnPositionChanged).AddTo(this);
            model.Activity.Subscribe(OnActivityChanged).AddTo(this);
            //
            model.Attacking += OnAttacking;
            model.Hurt += OnHurt;
            model.Die += OnDie;
        }

        protected void OnModelRemoved(BaseFighter model)
        {
            model.Attacking -= OnAttacking;
            model.Hurt -= OnHurt;
            model.Die -= OnDie;
        }

        protected void ToggleHealthBarVisibility(bool show)
        {
            if (simpleHealthBarRoot is {}) 
                simpleHealthBarRoot.SetActive(show);
        }

        private void OnHitPointsChanged(int hitPoints, int maxHp)
        {
            if (simpleHealthBar is { } && simpleHealthBarRoot.activeSelf)
                simpleHealthBar.UpdateBar(hitPoints, maxHp);
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

        protected abstract void OnAttacking(BaseFighter target);

        protected abstract void OnHurt();

        protected abstract void OnDie();
    }
}