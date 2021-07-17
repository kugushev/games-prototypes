using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace Kugushev.Scripts
{
    public class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var direction = new int2();
            if (Keyboard.current.rightArrowKey.isPressed)
                direction.x = 1;
            else if (Keyboard.current.leftArrowKey.isPressed)
                direction.x = -1;

            if (Keyboard.current.upArrowKey.isPressed)
                direction.y = 1;
            else if (Keyboard.current.downArrowKey.isPressed)
                direction.y = -1;


            Entities
                .ForEach((ref MoveData moveData) => moveData.Direction = direction)
                .Run();
        }
    }
}