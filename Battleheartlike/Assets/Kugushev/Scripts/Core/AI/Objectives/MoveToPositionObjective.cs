using Kugushev.Scripts.Common.Pooling;
using Kugushev.Scripts.Core.AI.Objectives.Abstractions;
using Kugushev.Scripts.Core.ValueObjects;

namespace Kugushev.Scripts.Core.AI.Objectives
{
    public class MoveToPositionObjective: Poolable<Position>, IObjective
    {
        public MoveToPositionObjective(ObjectsPool objectsPool) : base(objectsPool)
        {
        }
        
        // todo: add abstract Objective and save Active to this class (the same as interaction).  
    }
}