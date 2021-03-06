using Kugushev.Scripts.Common.Utils.Pooling;

namespace Kugushev.Scripts.Mission.Entities
{
    public class Mission: Poolable<Mission.State>
    {
        public struct State
        {
            
        }

        public Mission(ObjectsPool objectsPool) : base(objectsPool)
        {
        }
        
        
    }
}