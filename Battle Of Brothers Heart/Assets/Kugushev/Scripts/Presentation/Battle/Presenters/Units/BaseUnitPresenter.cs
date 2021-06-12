using Kugushev.Scripts.Core.Battle;
using Kugushev.Scripts.Core.Battle.Enums;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Battle.Presenters.Units
{
    public abstract class BaseUnitPresenter : MonoBehaviour
    {
        private const int TopLayerIndex = 0;

        private static readonly int SpeedAnimationParameter = Animator.StringToHash("Speed");
        private static readonly int SwingAnimationParameter = Animator.StringToHash("Swing");
        private static readonly int HurtAnimationParameter = Animator.StringToHash("Hurt");
        private static readonly int IdleAnimationParameter = Animator.StringToHash("Idle");
        private static readonly int DeathAnimationParameter = Animator.StringToHash("Death");

        [SerializeField] private SimpleHealthBar simpleHealthBar = default!;

        [Header("Character")] [SerializeField] private GameObject upObject = default!;
        [SerializeField] private Animator upAnimator = default!;
        [SerializeField] private GameObject leftObject = default!;
        [SerializeField] private Animator leftAnimator = default!;
        [SerializeField] private GameObject rightObject = default!;
        [SerializeField] private Animator rightAnimator = default!;
        [SerializeField] private GameObject downObject = default!;
        [SerializeField] private Animator downAnimator = default!;

        private Animator? _activeAnimator;

        public abstract BaseUnit Model { get; }

        private void Start()
        {
            _activeAnimator = downAnimator;

            Model.HitPoints.Subscribe(OnHitPointsChanged).AddTo(this);
            Model.Position.Subscribe(OnPositionChanged).AddTo(this);
            Model.Direction.Subscribe(OnDirectionChanged).AddTo(this);
            Model.Activity.Subscribe(OnActivityChanged).AddTo(this);

            Model.Attacking += OnAttacking;
            Model.AttackCanceled += OnAttackCanceled;
            Model.Hurt += OnHurt;
            Model.Die += OnDie;

            OnStart();
        }

        protected virtual void OnStart()
        {
        }

        private void OnDestroy()
        {
            OnDestruction();
            Model.Attacking -= OnAttacking;
            Model.AttackCanceled -= OnAttackCanceled;
            Model.Hurt -= OnHurt;
            Model.Die -= OnDie;
        }

        protected virtual void OnDestruction()
        {
        }

        private void OnHitPointsChanged(int hitPoints)
        {
            simpleHealthBar.UpdateBar(hitPoints, BattleConstants.UnitMaxHitPoints);
        }

        private void OnPositionChanged(Position newPosition)
        {
            var t = transform;

            Vector3 vector = newPosition.Vector;
            vector.z = t.position.z; // keep z position
            t.position = vector;
        }

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
                _activeAnimator.Play(SwingAnimationParameter, TopLayerIndex);
        }

        private void OnAttackCanceled()
        {
            if (_activeAnimator is { } && _activeAnimator.HasState(TopLayerIndex, SwingAnimationParameter))
            {
                _activeAnimator.Play(IdleAnimationParameter, TopLayerIndex);
            }
        }

        private void OnHurt(BaseUnit attacker)
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(HurtAnimationParameter, TopLayerIndex);
        }

        private void OnDie()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(DeathAnimationParameter, TopLayerIndex);
        }
    }
}