using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class Fist : MonoBehaviour
    {
        private readonly List<Rigidbody> _buffer = new List<Rigidbody>();

        private void OnTriggerEnter(Collider other)
        {
            _buffer.Clear();
            other.GetComponents(_buffer);

            if (_buffer.Count != 1)
            {
                Debug.LogError("Unexpected collision");
                return;
            }

            var rig = _buffer[0];


            rig.AddForce(transform.rotation.eulerAngles * 10);

            _buffer.Clear();
        }
    }
}