using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class Fist : MonoBehaviour
    {
        private Vector3 _position;
        public float Velocity { get; private set; }

        private void Awake()
        {
            _position = transform.position;
        }

        private void FixedUpdate()
        {
            var nextPosition = transform.position;
            Velocity = (nextPosition - _position).magnitude;
            _position = nextPosition;
        }
    }
}