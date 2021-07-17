using Unity.Entities;
using Unity.Mathematics;

namespace Kugushev.Scripts
{
    [GenerateAuthoringComponent]
    public struct MoveData: IComponentData
    {
        public int2 Direction;
        public float Speed;
        public float TurnSpeed;
    }
}