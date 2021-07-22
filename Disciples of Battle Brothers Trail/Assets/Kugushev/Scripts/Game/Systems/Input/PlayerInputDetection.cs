using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Enums;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kugushev.Scripts.Game.Systems.Input
{
    public class PlayerInputDetection : IEcsRunSystem
    {
        private EcsFilter<UnitMove, HeroUnitViewRef> _filter;

        public void Run()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                Debug.LogError("No Keyboard Found");
                return;
            }

            var direction = Direction2d.None;

            if (keyboard.rightArrowKey.isPressed)
                direction = Direction2d.Right;
            else if (keyboard.leftArrowKey.isPressed)
                direction = Direction2d.Left;
            else if (keyboard.upArrowKey.isPressed)
                direction = Direction2d.Up;
            else if (keyboard.downArrowKey.isPressed)
                direction = Direction2d.Down;

            foreach (var i in _filter)
            {
                ref var move = ref _filter.Get1(i);
                move.Direction = direction;
            }
        }
    }
}