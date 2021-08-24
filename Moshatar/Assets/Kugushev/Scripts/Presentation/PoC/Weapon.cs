using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


namespace Kugushev.Scripts.Presentation.PoC
{
    public class Weapon : MonoBehaviour
    {
        public const float MaxDurability = 10f;
        public const int WeaponDamage = 10;
        public const int WeaponCrit = 50;
        
        private XRController _xrController;
        private Vector3 _position;
        private readonly List<AttackDirection> _attacksStack = new List<AttackDirection>(8);

        public float Velocity { get; private set; }
        public ReactiveProperty<float> WeaponDurability { get; } = new ReactiveProperty<float>(1);

        public int RegisterWeaponHit(bool isHardHit, int power)
        {
            if (!isHardHit)
                return 0;

            return WeaponDamage * (power / 10);
        }

        public Combo RegisterWeaponHitFinished(AttackDirection attackDirection)
        {
            //WeaponDurability.Value -= 1f;
            _attacksStack.Add(attackDirection);

            if (_attacksStack.Count >= 2)
            {
                var last = _attacksStack.Last();
                var lastBefore = _attacksStack[_attacksStack.Count - 2];

                if (last == AttackDirection.DiagonalDown && lastBefore == AttackDirection.DiagonalDown)
                    return new Combo(WeaponDamage, DamageEffect.Bleed);
            }

            if (_attacksStack.Count >= 3)
            {
                var last = _attacksStack.Last();
                var lastBefore = _attacksStack[_attacksStack.Count - 2];
                var lastBeforeBefore = _attacksStack[_attacksStack.Count - 2];

                if (last == AttackDirection.VerticalUp && lastBefore == AttackDirection.VerticalUp &&
                    lastBeforeBefore == AttackDirection.VerticalUp)
                    return new Combo(WeaponCrit, DamageEffect.Purge);
            }

            return new Combo(WeaponDamage);
        }

        protected void Awake()
        {
            _xrController = GetComponentInParent<XRController>();
            if (_xrController is null)
            {
                Debug.LogError("Can't find controller");
                return;
            }

            _position = transform.position;
        }

        protected void FixedUpdate()
        {
            var nextPosition = transform.position;
            Velocity = (nextPosition - _position).magnitude;
            _position = nextPosition;
        }
    }
}