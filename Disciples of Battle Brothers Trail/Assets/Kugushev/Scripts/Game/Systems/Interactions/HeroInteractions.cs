using Kugushev.Scripts.Common.Managers;
using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Models;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Game.Systems.Interactions
{
    public class HeroInteractions : IEcsRunSystem
    {
        private EcsFilter<UnitGridPosition, HeroUnitViewRef> _filter;
        private World _world;
        private GameModeManager _gameModeManager;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);

                if (!position.Moving && !position.Stopped)
                {
                    var cell = _world.GetCell(position.ActualPosition);
                    if (cell is CityWorldCell) 
                        _gameModeManager.ToCityAsync();
                }
            }
        }
    }
}