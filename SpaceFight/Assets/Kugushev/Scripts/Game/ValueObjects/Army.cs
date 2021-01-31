using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Models.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Game.ValueObjects
{
    public class Army : Poolable<Army.State>
    {
        public Army(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public struct State
        {
            public readonly Order Order;
            public readonly float Speed;
            public readonly float AngularSpeed;
            public int Power;
            public bool Arrived;
            public Vector3 CurrentPosition;
            public Quaternion CurrentRotation;

            public State(Order order, float speed, float angularSpeed, int power)
            {
                // todo: assert order
                Order = order;
                Speed = speed;
                AngularSpeed = angularSpeed;
                Power = power;
                Arrived = false;
                CurrentPosition = order.Path[0];
                CurrentRotation = Quaternion.identity;
            }
        }

        public Vector3 Position => ObjectState.CurrentPosition;
        public Quaternion Rotation => ObjectState.CurrentRotation;
        public int Power => ObjectState.Power;
        public bool Disbanded => ObjectState.Arrived;

        public IEnumerator Send(Func<float> deltaTime)
        {
            // todo: use UniTask
            var previous = ObjectState.CurrentPosition;

            for (var i = 1; i < ObjectState.Order.Path.Count; i++)
            {
                var next = ObjectState.Order.Path[i];

                var rotationDelta = 0f;
                for (float delta = 0f; delta < 1f; delta += deltaTime() * ObjectState.Speed)
                {
                    ObjectState.CurrentPosition = Vector3.Lerp(previous, next, delta);

                    rotationDelta += deltaTime() * ObjectState.AngularSpeed;
                    var lookRotation = Quaternion.LookRotation(next - ObjectState.CurrentPosition);
                    ObjectState.CurrentRotation = Quaternion.Slerp(ObjectState.CurrentRotation, lookRotation, rotationDelta);

                    yield return null;
                    
                    if (ObjectState.Arrived)
                        yield break;
                }
                
                ObjectState.CurrentPosition = previous = next;
            }
        }

        public void HandlePlanetArriving(Planet planet)
        {
            if (planet == ObjectState.Order.SourcePlanet)
                return;

            switch (planet.Faction)
            {
                case Faction.Player:
                    planet.Reinforce(this);
                    break;
                case Faction.Neutral:
                case Faction.Enemy:
                    planet.Fight(this);
                    break;
                default:
                    Debug.LogError($"Unexpected planet faction {planet.Faction}");
                    break;
            }
            
            ObjectState.Arrived = true;
        }

        protected override void OnClear(State state)
        {
            state.Order.Dispose();
        }
    }
}