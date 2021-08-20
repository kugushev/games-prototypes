using UniRx;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Presentation.PoC
{
    [RequireComponent(typeof(VelocityMeasured))]
    public class Fist : MonoBehaviour
    {
        private const int FistDamage = 1;
        private const int FistHardDamage = 10;

        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private Material emptyDurability;
        [SerializeField] private Material filledDurability;
        [SerializeField] private Material fullDurability;
        [SerializeField] private Weapon weapon;

        private SphereCollider _collider;
        private XRController _xrController;
        private Material[] emptyDurabilityMaterials;
        private Material[] filledDurabilityMaterials;
        private Material[] fullDurabilityMaterials;

        private VelocityMeasured _velocityMeasured;

        public float Velocity => _velocityMeasured.Velocity;

        public int RegisterFistHit(bool isHardHit)
        {
            if (weapon.WeaponDurability.Value > Weapon.MaxDurability)
            {
                _xrController.SendHapticImpulse(1f, 1f);
            }
            else
            {
                if (!isHardHit)
                    weapon.WeaponDurability.Value += Random.Range(0, 0.1f);
                else
                    weapon.WeaponDurability.Value += 0.5f;
            }

            return isHardHit ? FistHardDamage : FistDamage;
        }


        protected void Awake()
        {
            _velocityMeasured = GetComponent<VelocityMeasured>();
            _collider = GetComponent<SphereCollider>();

            _xrController = GetComponentInParent<XRController>();
            if (_xrController is null)
            {
                Debug.LogError("Can't find controller");
                return;
            }

            emptyDurabilityMaterials = new[]
            {
                emptyDurability
            };

            filledDurabilityMaterials = new[]
            {
                filledDurability
            };

            fullDurabilityMaterials = new[]
            {
                fullDurability
            };

            weapon.WeaponDurability.Subscribe(WeaponDurabilityChanged).AddTo(this);
        }

        protected void Update()
        {
            var inputDevice = _xrController.inputDevice;
            if (inputDevice.IsPressed(InputHelpers.Button.Grip, out var isPressed) && isPressed &&
                weapon.WeaponDurability.Value >= 1f)
            {
                weapon.gameObject.SetActive(true);
                _collider.enabled = false;
            }
            else
            {
                weapon.gameObject.SetActive(false);
                _collider.enabled = true;
            }
        }
        
        private void WeaponDurabilityChanged(float durability)
        {
            if (durability < 1f)
                meshRenderer.materials = emptyDurabilityMaterials;
            else if (durability < Weapon.MaxDurability)
                meshRenderer.materials = filledDurabilityMaterials;
            else
                meshRenderer.materials = fullDurabilityMaterials;
        }
    }
}