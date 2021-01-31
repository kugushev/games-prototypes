using System;
using Kugushev.Scripts.Game.ValueObjects;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Kugushev.Scripts.Presentation.Components
{
    public class ArmyPresenter: MonoBehaviour
    {
        private FleetPresenter _owner;

        public void SetOwner(FleetPresenter owner)
        {
            if (!ReferenceEquals(_owner, null)) 
                Debug.LogError("Fleet is already specified");

            _owner = owner;
        }

        public Army Army { get; internal set; }

        public void Send()
        {
            StartCoroutine(Army.Send(() => Time.deltaTime));
        }

        private void Update()
        {
            var t = transform;
            
            t.position = Army.Position;
            t.rotation = Army.Rotation;

            if (Army.Disbanded) 
                _owner.ReturnArmyToPool(this);
        }
    }
}