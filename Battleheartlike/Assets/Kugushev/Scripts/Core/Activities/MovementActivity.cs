using Kugushev.Scripts.Common.Pooling;
using Kugushev.Scripts.Core.Activities.Abstractions;
using Kugushev.Scripts.Core.AI.Objectives;
using Kugushev.Scripts.Core.AI.Objectives.Abstractions;
using Kugushev.Scripts.Core.Behaviors;
using Kugushev.Scripts.Core.ValueObjects;

namespace Kugushev.Scripts.Core.Activities
{
    public class MovementActivity: Activity<IMovable, Position>
    {
        public MovementActivity(ObjectsPool objectsPool) : base(objectsPool) { }

        public override IObjective Act()
        {
            return ObjectState.Active.PathfindingProvider.TestDestination(ObjectState.State) 
                ? MyPool.GetObject<MoveToPositionObjective, Position>(ObjectState.State) 
                : null;
        }
    }
}