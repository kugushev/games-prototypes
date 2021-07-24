using Kugushev.Scripts.Common.Managers;
using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Models.CityInfo;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Game.Systems.Interactions
{
    internal class HeroInteractions : IEcsRunSystem
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
                    if (cell is CityWorldCell cityCell)
                    {
                        City.VisitedCity = cityCell.City;

                        _gameModeManager.ToCityAsync();
                    }
                }
            }
        }
    }
}