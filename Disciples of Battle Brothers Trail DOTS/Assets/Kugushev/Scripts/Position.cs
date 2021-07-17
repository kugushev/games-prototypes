using Unity.Entities;
using Unity.Mathematics;

namespace Kugushev.Scripts
{
    public readonly struct Position: IComponentData
    {
        public readonly float2 Value;

        public Position(float2 value) => Value = value;
    }
}