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
    public abstract class BaseUnitPresenter : MonoBehaviour
    {
        private static readonly int SpeedAnimationParameter = Animator.StringToHash("Speed");
        private static readonly int SwingAnimationParameter = Animator.StringToHash("Swing");
        private static readonly int HurtAnimationParameter = Animator.StringToHash("Hurt");

        [Header("Character")] [SerializeField] private GameObject upObject = default!;
        [SerializeField] private Animator upAnimator = default!;
        [SerializeField] private GameObject leftObject = default!;
        [SerializeField] private Animator leftAnimator = default!;
        [SerializeField] private GameObject rightObject = default!;
        [SerializeField] private Animator rightAnimator = default!;
        [SerializeField] private GameObject downObject = default!;
        [SerializeField] private Animator downAnimator = default!;

        private Animator? _activeAnimator;

        protected abstract BaseUnit Model { get; }

        private void Awake()
        {
            _activeAnimator = downAnimator;

            Model.Position.Subscribe(OnPositionChanged).AddTo(this);
            Model.Direction.Subscribe(OnDirectionChanged).AddTo(this);
            Model.Activity.Subscribe(OnActivityChanged).AddTo(this);
            Model.Attacking += OnAttacking;
            Model.Hurt += OnHurt;

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        private void OnDestroy()
        {
            OnDestruction();
            Model.Attacking -= OnAttacking;
            Model.Hurt += OnHurt;
        }

        protected virtual void OnDestruction()
        {
        }


        private void OnPositionChanged(Position newPosition) => transform.position = newPosition.Vector;

        private void OnDirectionChanged(UnitDirection newDirection)
        {
            switch (newDirection)
            {
                case UnitDirection.Up:
                    upObject.SetActive(true);
                    rightObject.SetActive(false);
                    leftObject.SetActive(false);
                    downObject.SetActive(false);

                    _activeAnimator = upAnimator;
                    ToggleActivity(Model.Activity.Value);
                    break;
                case UnitDirection.Down:
                    upObject.SetActive(false);
                    rightObject.SetActive(false);
                    leftObject.SetActive(false);
                    downObject.SetActive(true);

                    _activeAnimator = downAnimator;
                    ToggleActivity(Model.Activity.Value);
                    break;
                case UnitDirection.Right:
                    upObject.SetActive(false);
                    rightObject.SetActive(true);
                    leftObject.SetActive(false);
                    downObject.SetActive(false);

                    _activeAnimator = rightAnimator;
                    ToggleActivity(Model.Activity.Value);
                    break;
                case UnitDirection.Left:
                    upObject.SetActive(false);
                    rightObject.SetActive(false);
                    leftObject.SetActive(true);
                    downObject.SetActive(false);

                    _activeAnimator = leftAnimator;
                    ToggleActivity(Model.Activity.Value);
                    break;
                default:
                    Debug.LogError($"Unexpected direction {newDirection}");
                    break;
            }
        }

        private void OnActivityChanged(UnitActivity newActivity) => ToggleActivity(newActivity);

        private void ToggleActivity(UnitActivity activity)
        {
            var speed = activity == UnitActivity.Move ? BattleConstants.UnitSpeed : 0;
            if (_activeAnimator is { })
                _activeAnimator.SetFloat(SpeedAnimationParameter, speed);
        }

        private void OnAttacking()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(SwingAnimationParameter, 0);
        }

        private void OnHurt()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(HurtAnimationParameter, 0);
        }
    }
}