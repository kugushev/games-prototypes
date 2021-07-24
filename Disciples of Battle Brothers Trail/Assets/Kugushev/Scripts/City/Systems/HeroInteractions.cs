using System;
using Kugushev.Scripts.City.Components;
using Kugushev.Scripts.City.Models.Interactables;
using Kugushev.Scripts.Common.Managers;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.City.Systems
{
    public class HeroInteractions : IEcsRunSystem
    {
        private EcsFilter<HeroInteractCommand> _filter;
        private GameModeManager _gameModeManager;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var interactCommand = ref _filter.Get1(i);

                switch (interactCommand.Interactable)
                {
                    case ExitZone _:
                        _gameModeManager.ToGameAsync();
                        break;
                    case HiringDesk hiringDesk:
                        Debug.Log("Start hiring");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}