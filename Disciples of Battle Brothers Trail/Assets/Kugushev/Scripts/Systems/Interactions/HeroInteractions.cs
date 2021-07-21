using Kugushev.Scripts.Common;
using Kugushev.Scripts.Components;
using Kugushev.Scripts.Components.ViewRefs;
using Kugushev.Scripts.Models;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Systems.Interactions
{
    [CampaignSystem]
    public class HeroInteractions : IEcsRunSystem
    {
        private EcsFilter<UnitGridPosition, HeroUnitViewRef> _filter;
        private World _world;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);

                if (!position.Moving && !position.Stopped)
                {
                    var cell = _world.GetCell(position.ActualPosition);
                    if (cell is CityWorldCell)
                    {
                        Debug.Log("Visit City");
                    }
                }
            }
        }
    }
}