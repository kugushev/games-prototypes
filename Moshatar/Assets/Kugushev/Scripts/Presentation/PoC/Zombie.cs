using System;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class Zombie: MonoBehaviour
    {
        [SerializeField] private Transform victim;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var position = transform.position;
            var direction = (victim.position - position).normalized * 0.01f;
            
            
            _rigidbody.MovePosition(position + direction);
        }
    }
}