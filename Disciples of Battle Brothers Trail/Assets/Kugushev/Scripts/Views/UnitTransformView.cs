using Kugushev.Scripts.Enums;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kugushev.Scripts.Views
{
    public class UnitTransformView : MonoBehaviour
    {
        private const int TopLayerIndex = 0;

        private static readonly int SpeedAnimationParameter = Animator.StringToHash("Speed");
        private static readonly int SwingAnimationParameter = Animator.StringToHash("Swing");
        private static readonly int HurtAnimationParameter = Animator.StringToHash("Hurt");
        private static readonly int IdleAnimationParameter = Animator.StringToHash("Idle");
        private static readonly int DeathAnimationParameter = Animator.StringToHash("Death");

        [Header("Character")] [SerializeField] private GameObject downObject;
        [SerializeField] private Animator downAnimator;
        [SerializeField] private GameObject upObject;
        [SerializeField] private Animator upAnimator;
        [SerializeField] private GameObject leftObject;
        [SerializeField] private Animator leftAnimator;
        [SerializeField] private GameObject rightObject;
        [SerializeField] private Animator rightAnimator;

        private Animator _activeAnimator;

        private void Start()
        {
            _activeAnimator = downAnimator;
        }


        public void UpdatePosition(Vector3 newPosition)
        {
            var t = transform;

            Vector3 vector = newPosition;
            vector.z = t.position.z; // keep z position
            t.position = vector;
        }

        public void UpdateDirection(Direction2d newDirection2d)
        {
            switch (newDirection2d)
            {
                case Direction2d.None:
                    break;

                case Direction2d.Up:
                    upObject.SetActive(true);
                    rightObject.SetActive(false);
                    leftObject.SetActive(false);
                    downObject.SetActive(false);

                    _activeAnimator = upAnimator;
                    break;
                case Direction2d.Down:
                    upObject.SetActive(false);
                    rightObject.SetActive(false);
                    leftObject.SetActive(false);
                    downObject.SetActive(true);

                    _activeAnimator = downAnimator;
                    break;
                case Direction2d.Right:
                    upObject.SetActive(false);
                    rightObject.SetActive(true);
                    leftObject.SetActive(false);
                    downObject.SetActive(false);

                    _activeAnimator = rightAnimator;
                    break;
                case Direction2d.Left:
                    upObject.SetActive(false);
                    rightObject.SetActive(false);
                    leftObject.SetActive(true);
                    downObject.SetActive(false);

                    _activeAnimator = leftAnimator;
                    break;
                default:
                    Debug.LogError($"Unexpected direction {newDirection2d}");
                    break;
            }
        }

        public void UpdateIsMoving(bool isMoving)
        {
            var speed = isMoving ? 20 : 0;
            if (_activeAnimator is { })
                _activeAnimator.SetFloat(SpeedAnimationParameter, speed);
        }


        // private void OnAttacking()
        // {
        //     if (_activeAnimator is { })
        //         _activeAnimator.Play(SwingAnimationParameter, TopLayerIndex);
        // }
        //
        // private void OnAttackCanceled()
        // {
        //     if (_activeAnimator is { } && _activeAnimator.HasState(TopLayerIndex, SwingAnimationParameter))
        //     {
        //         _activeAnimator.Play(IdleAnimationParameter, TopLayerIndex);
        //     }
        // }

        // private void OnHurt(BaseFighter attacker)
        // {
        //     if (_activeAnimator is { })
        //         _activeAnimator.Play(HurtAnimationParameter, TopLayerIndex);
        // }

        private void OnDie()
        {
            if (_activeAnimator is { })
                _activeAnimator.Play(DeathAnimationParameter, TopLayerIndex);
        }
    }
}