using Kugushev.Scripts.City.Components;
using Kugushev.Scripts.City.Components.Commands;
using Kugushev.Scripts.City.Systems;
using Kugushev.Scripts.City.Views;
using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Common.Managers;
using Kugushev.Scripts.Common.UI;
using Kugushev.Scripts.Game.Components.Commands;
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
        [SerializeField] private ModalMenuManager modalMenuManager;
        [SerializeField] private UnitTransformView heroUnit;

        protected override void InitSystems(EcsSystems ecsSystems)
        {
            ecsSystems
                .Add(new ProcessHeroMove())
                .Add(new PlayerInputDetection())
                .Add(new UnitUpdateTransformView())
                .Add(new UnitAnimateMoving(CityConstants.UnitSpeed))
                .Add(new CitizensSpawner())
                .Add(new HeroInteractions());

            ecsSystems
                .OneFrame<MoveCommand>()
                .OneFrame<InteractCommand>();
        }

        protected override void Inject(EcsSystems ecsSystems)
        {
            ecsSystems.Inject(Hero.Instance);

            var city = new Models.CityStructure();
            cityView.Init(city);
            ecsSystems.Inject(city);
            ecsSystems.Inject(cityView);

            ecsSystems.Inject(modalMenuManager);

            ecsSystems.Inject(heroUnit);

            ecsSystems.Inject(GameModeManager.Instance);
        }
    }
}