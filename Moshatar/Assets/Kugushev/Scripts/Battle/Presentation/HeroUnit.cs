using System;
using System.Collections;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using Kugushev.Scripts.Core.Services;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation
{
    public class HeroUnit : MonoBehaviour
    {
        [SerializeField] private AudioSource hitSound;
        [Inject] private GameModeService _gameModeService;

        private readonly WaitForSeconds _waitForHeal = new WaitForSeconds(5f);

        public HeroFighter Model { get; private set; }

        public void Init(HeroFighter model)
        {
            Model = model;
            OnModelSet(Model);
        }

        public event Action<int, int> Hurt;
        
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
            Hurt?.Invoke(Model.Character.HP.Value, Model.Character.MaxHP);
            hitSound.Play();
        }

        private void OnDie() => _gameModeService.BackToMenu();
    }
}