using Kugushev.Scripts.City.Systems;
using Kugushev.Scripts.City.Views;
using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Common.Managers;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Game.Systems;
using Kugushev.Scripts.Game.Systems.AI;
using Kugushev.Scripts.Game.Systems.CommandsProcessing;
using Kugushev.Scripts.Game.Systems.Input;
using Kugushev.Scripts.Game.Systems.Interactions;
using Kugushev.Scripts.Game.Systems.UpdateView;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.City
{
    public class CityRoot : BaseRoot
    {
        [SerializeField] private CityView cityView;
        [SerializeField] private UnitTransformView heroUnit;
        
        protected override void InitSystems(EcsSystems ecsSystems)
        {
            ecsSystems
                .Add(new ProcessUnitMove())
                .Add(new PlayerInputDetection())
                .Add(new UnitUpdateTransformView())
                .Add(new UnitAnimateMoving(CityConstants.UnitSpeed))
                .Add(new CitizensSpawner())
                .Add(new HeroInteractions());
        }

        protected override void Inject(EcsSystems ecsSystems)
        {
            ecsSystems.Inject(Hero.Instance);

            var city = new Models.City();
            cityView.Init(city);
            ecsSystems.Inject(city);
            ecsSystems.Inject(cityView);
            
            ecsSystems.Inject(heroUnit);

            // todo: start using Zenject
            var gameModeManager = FindObjectOfType<GameModeManager>();
            ecsSystems.Inject(gameModeManager);
        }
    }
}