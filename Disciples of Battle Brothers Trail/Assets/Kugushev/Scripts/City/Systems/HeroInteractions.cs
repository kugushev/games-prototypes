using System;
using Kugushev.Scripts.City.Models.Cells;
using Kugushev.Scripts.City.Models.Interactables;
using Kugushev.Scripts.Common.Managers;
using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.City.Systems
{
    public class HeroInteractions : IEcsRunSystem
    {
        private EcsFilter<UnitGridPosition, HeroUnitViewRef> _filter;
        private Models.City _city;
        private GameModeManager _gameModeManager;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);
                var p = position.ActualPosition;

                var cell = _city.Grid[p.y][p.x];

                switch (cell)
                {
                    case EmptyPavementCell emptyPavementCell:
                        break;
                    case InteractableCell interactableCell:
                        switch (interactableCell.Interactable)
                        {
                            case ExitZone exitZone:
                                _gameModeManager.ToGameAsync();
                                break;
                            case HiringDesk hiringDesk:
                                Debug.Log("Start hiring");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(cell));
                }
            }
        }
    }
}