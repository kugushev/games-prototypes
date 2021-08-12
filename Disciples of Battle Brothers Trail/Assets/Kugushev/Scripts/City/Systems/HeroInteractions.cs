using System;
using Kugushev.Scripts.City.Components.Commands;
using Kugushev.Scripts.City.Models.Interactables;
using Kugushev.Scripts.City.UI;
using Kugushev.Scripts.Common.Ecs.Components;
using Kugushev.Scripts.Common.UI;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Leopotam.Ecs;

namespace Kugushev.Scripts.City.Systems
{
    public class HeroInteractions : IEcsRunSystem
    {
        private EcsFilter<InteractCommand> _filter;
        private GameModeManager _gameModeManager;
        private ModalMenuManager _modalMenuManager;
        private Game.Models.CityInfo.CityWorldItem _currentCityWorldItem;
        private Hero _hero;

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
                        var menu = _modalMenuManager.OpenModalMenu<HiringMenu>();
                        menu.Init(hiringDesk.HiringDeskInfo, _hero);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var entity = _filter.GetEntity(i);
                entity.Del<InteractionsLock>();
            }
        }
    }
}