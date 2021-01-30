using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Models.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public class Army: Poolable<Army.State>
    {
        public struct State
        {
            public readonly Order Order;
            public readonly float Speed;
            public Vector3 CurrentPosition;
            public int Power;
            public bool Arrived;

            public State(Order order, float speed, int power)
            {
                // todo: assert order
                Order = order;
                Speed = speed;
                Power = power;
                Arrived = false;
                CurrentPosition = order.Path[0];
            }
        }

        public Army(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public Vector3 Position => ObjectState.CurrentPosition;
        public bool Arrived => ObjectState.Arrived;

        public IEnumerator Send(Func<float> deltaTime)
        {
            var start = ObjectState.CurrentPosition;

            for (var i = 1; i < ObjectState.Order.Path.Count; i++)
            {
                var next = ObjectState.Order.Path[i];
                for (float delta = 0f; delta < 1f; delta += deltaTime() * ObjectState.Speed)
                {
                    ObjectState.CurrentPosition = Vector3.Lerp(start, next, delta);
                    yield return null;
                }

                start = next;
            }

            ObjectState.Arrived = true;
        }
        
        protected override void OnClear(State state)
        {
            state.Order.Dispose();
        }
    }
}