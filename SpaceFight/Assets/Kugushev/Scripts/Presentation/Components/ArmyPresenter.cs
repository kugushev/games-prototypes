using System;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Kugushev.Scripts.Presentation.Components
{
    public class ArmyPresenter: MonoBehaviour
    {
        public Army Army { get; set; }

        private void Start()
        {
            StartCoroutine(Army.Send(() => Time.deltaTime));
            
        }

        private void Update()
        {
            transform.position = Army.Position;

            if (Army.Arrived)
            {
                // todo: handle properly (with pooling)
                Destroy(gameObject);
            }
        }
    }
}