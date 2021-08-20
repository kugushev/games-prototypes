using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class VelocityMeasured : MonoBehaviour
    {
        private Vector3 _position;

        public float Velocity { get; private set; }

        protected void FixedUpdate()
        {
            var nextPosition = transform.position;
            Velocity = (nextPosition - _position).magnitude;
            _position = nextPosition;
        }
    }
}