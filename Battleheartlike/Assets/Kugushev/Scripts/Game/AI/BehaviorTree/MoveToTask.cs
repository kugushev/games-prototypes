using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.AI.BehaviorTree.Abstractions;
using Kugushev.Scripts.Game.Features;
using UnityEngine;

namespace Kugushev.Scripts.Game.AI.BehaviorTree
{
    [Serializable]
    internal class MoveToTask : BehaviorTreeTask<MoveToTask.State>
    {
        public struct State
        {
            public IMovable Agent { get; }
            public Position Destination { get; }

            public State(IMovable agent, Position destination)
            {
                Agent = agent;
                Destination = destination;
            }

            public void Deconstruct(out IMovable agent, out Position destination)
            {
                agent = Agent;
                destination = Destination;
            }
        }

        public MoveToTask(ObjectsPool objectsPool)
            : base(objectsPool)
        {
        }

        public override async UniTask<bool> RunAsync(CancellationToken cancellationToken)
        {
            var (agent, destination) = ObjectState;

            if (!agent.NavigationService.TrySetDestination(in destination))
                return false;

            agent.IsMoving = true;

            await UniTask.WaitUntil(DestinationReached, cancellationToken: cancellationToken);

            agent.IsMoving = false;

            return true;
        }

        private bool DestinationReached() => ObjectState.Agent?.NavigationService.TestIfDestinationReached() ?? false;
    }
}