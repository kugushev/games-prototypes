using System;
using System.Collections;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation
{
    public class HeroUnit : MonoBehaviour
    {
        private readonly WaitForSeconds _waitForHeal = new WaitForSeconds(5f);

        public HeroFighter Model { get; private set; }

        public void Init(HeroFighter model)
        {
            Model = model;
            OnModelSet(Model);
        }

        public event Action<int, int> Hurt;

        // todo: handle hit (head collider)
        // todo: add invulnarable time
        // todo: handle death
        // todo: hp add regen

        private IEnumerator Start()
        {
            while (true)
            {
                yield return _waitForHeal;
                if (Model != null)
                    Model.Regenerate();
            }
        }

        private void Update()
        {
            if (Model == null)
                return;

            var p = transform.position;

            var v = new Vector2(p.x, p.z); // as intended - just translate 2D to 3D
            Model.UpdatePosition(new Position(v));
        }

        private void OnModelSet(HeroFighter model)
        {
            model.Hurt += OnHurt;
            model.Die += OnDie;
        }

        private void OnHurt()
        {
            Debug.Log($"Hurt: {Model.Character.HP.Value}");
            Hurt?.Invoke(Model.Character.HP.Value, Model.Character.MaxHP);
        }

        private void OnDie()
        {
        }
    }
}