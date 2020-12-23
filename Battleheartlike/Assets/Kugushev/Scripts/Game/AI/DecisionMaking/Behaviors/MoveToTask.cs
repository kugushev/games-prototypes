using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors.Abstractions;
using Kugushev.Scripts.Game.Features;

namespace Kugushev.Scripts.Game.AI.DecisionMaking.Behaviors
{
    [Serializable]
    internal class MoveToTask: Poolable<MoveToTask.State>, IBehaviorTreeTask
    {
        public readonly struct State
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

        public async UniTask<bool> RunAsync()
        {
            var (agent, destination) = ObjectState;
            
            if (!agent.NavigationComponent.TrySetDestination(in destination))
                return false;

            agent.IsMoving = true;

            await UniTask.WaitUntil(DestinationReached);
            
            agent.IsMoving = false;

            return true;
        }

        private bool DestinationReached() => ObjectState.Agent.NavigationComponent.TestIfDestinationReached();
    }
}