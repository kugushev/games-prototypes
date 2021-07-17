using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Kugushev.Scripts.Components.ViewRefs;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kugushev.Scripts.Systems
{
    [CampaignSystem]
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

            var direction = int2.zero;

            if (keyboard.rightArrowKey.isPressed)
                direction.x = 1;
            else if (keyboard.leftArrowKey.isPressed)
                direction.x = -1;

            if (keyboard.upArrowKey.isPressed)
                direction.y = 1;
            else if (keyboard.downArrowKey.isPressed)
                direction.y = -1;


            foreach (var i in _filter)
            {
                ref var move = ref _filter.Get1(i);
                move.Direction = direction;
            }
        }
    }
}