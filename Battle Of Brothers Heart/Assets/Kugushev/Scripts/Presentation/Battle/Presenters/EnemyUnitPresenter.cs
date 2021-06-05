using System;
using Kugushev.Scripts.Core.Battle.Models;
using Kugushev.Scripts.Presentation.Battle.Controllers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.Battle.Presenters
{
    public class EnemyUnitPresenter : MonoBehaviour
    {
        private static readonly int HurtAnimationParameter = Animator.StringToHash("Hurt");

        [Header("Character")] [SerializeField] private GameObject upObject = default!;
        [SerializeField] private Animator upAnimator = default!;
        [SerializeField] private GameObject leftObject = default!;
        [SerializeField] private Animator leftAnimator = default!;
        [SerializeField] private GameObject rightObject = default!;
        [SerializeField] private Animator rightAnimator = default!;
        [SerializeField] private GameObject downObject = default!;
        [SerializeField] private Animator downAnimator = default!;

        [Inject] private SquadController _squadController = default!;

        private readonly EnemyUnit _model = new EnemyUnit();

        private void Awake()
        {
            _model.Hurt += OnHurt;
        }


        private void OnDestroy()
        {
            _model.Hurt -= OnHurt;
        }


        public void Clicked() => _squadController.EnemyUnitClicked(_model);

        private void OnHurt()
        {
            if (upAnimator.gameObject.activeSelf)
                upAnimator.Play(HurtAnimationParameter, 0);
            if (leftAnimator.gameObject.activeSelf)
                leftAnimator.Play(HurtAnimationParameter, 0);
            if (rightAnimator.gameObject.activeSelf)
                rightAnimator.Play(HurtAnimationParameter, 0);
            if (downAnimator.gameObject.activeSelf)
                downAnimator.Play(HurtAnimationParameter, 0);
        }
    }
}