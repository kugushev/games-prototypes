using Kugushev.Scripts.Common.Core.Enums;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Simple_Health_Bar.Scripts;
using UnityEngine;

namespace Kugushev.Scripts.Common.Presentation.ReactivePresentationModels
{
    public abstract class BaseCharacterRPM : MonoBehaviour
    {
        private const int TopLayerIndex = 0;

        private static readonly int SpeedAnimationParameter = Animator.StringToHash("Speed");
        private static readonly int SwingAnimationParameter = Animator.StringToHash("Swing");
        private static readonly int HurtAnimationParameter = Animator.StringToHash("Hurt");
        private static readonly int IdleAnimationParameter = Animator.StringToHash("Idle");
        private static readonly int DeathAnimationParameter = Animator.StringToHash("Death");

        [Header("Character")] [SerializeField] private GameObject upObject = default!;
        [SerializeField] private Animator upAnimator = default!;
        [SerializeField] private GameObject leftObject = default!;
        [SerializeField] private Animator leftAnimator = default!;
        [SerializeField] private GameObject rightObject = default!;
        [SerializeField] private Animator rightAnimator = default!;
        [SerializeField] private GameObject downObject = default!;
        [SerializeField] private Animator downAnimator = default!;

        private Animator? _activeAnimator;


        private void Start()
        {
            _activeAnimator = downAnimator;

            OnStart();
        }

        protected virtual void OnStart()
        {
        }

        private void OnDestroy()
        {
            OnDestruction();
        }

        protected virtual void OnDestruction()
        {
        }

        protected void OnPositionChanged(Position newPosition)
        {
            var t = transform;

            Vector3 vector = newPosition.Vector;
            vector.z = t.position.z; // keep z position
            t.position = vector;
        }

        protected void OnDirectionChanged(Direction2d newDirection2d)
        {
            switch (newDirection2d)
            {
                case Direction2d.Up:
                    upObject.SetActive(true);
                    rightObject.SetActive(false);
                    leftObject.SetActive(false);
                    downObject.SetActive(false);

                    _activeAnimator = upAnimator;
                    ToggleActivity();
                    break;
                case Direction2d.Down:
                    upObject.SetActive(false);
                    rightObject.SetActive(false);
                    leftObject.SetActive(false);
                    downObject.SetActive(true);

                    _activeAnimator = downAnimator;
                    ToggleActivity();
                    break;
                case Direction2d.Right:
                    upObject.SetActive(false);
                    rightObject.SetActive(true);
                    leftObject.SetActive(false);
                    downObject.SetActive(false);

                    _activeAnimator = rightAnimator;
                    ToggleActivity();
                    break;
                case Direction2d.Left:
                    upObject.SetActive(false);
                    rightObject.SetActive(false);
                    leftObject.SetActive(true);
                    downObject.SetActive(false);

                    _activeAnimator = leftAnimator;
                    ToggleActivity();
                    break;
                default:
                    Debug.LogError($"Unexpected direction {newDirection2d}");
                    break;
            }
        }

        protected abstract ActivityType CurrentActivity { get; }
        protected void OnActivityChanged(ActivityType newActivityType) => ToggleActivity(newActivityType);
        private void ToggleActivity(ActivityType? activityType = null)
        {
            if (activityType == null)
                activityType = CurrentActivity;

            var speed = activityType == ActivityType.Move ? 10 : 0;
            if (_activeAnimator is { })
                _activeAnimator.SetFloat(SpeedAnimationParameter, speed);
        }

        protected void OnAttacking()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(SwingAnimationParameter, TopLayerIndex);
        }

        protected void OnAttackCanceled()
        {
            if (_activeAnimator is { } && _activeAnimator.HasState(TopLayerIndex, SwingAnimationParameter))
            {
                _activeAnimator.Play(IdleAnimationParameter, TopLayerIndex);
            }
        }

        protected void OnHurt()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(HurtAnimationParameter, TopLayerIndex);
        }

        protected void OnDie()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(DeathAnimationParameter, TopLayerIndex);
        }
    }
}