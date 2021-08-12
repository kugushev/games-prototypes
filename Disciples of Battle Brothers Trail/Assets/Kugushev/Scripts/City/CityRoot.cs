using Kugushev.Scripts.City.Components.Commands;
using Kugushev.Scripts.City.Systems;
using Kugushev.Scripts.City.Views;
using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Common.UI;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models.CityInfo;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.Systems;
using Kugushev.Scripts.Game.Systems.Input;
using Kugushev.Scripts.Game.Systems.UpdateView;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.City
{
    public class CityRoot : BaseRoot
    {
        [SerializeField] private CityView cityView;
        [SerializeField] private UnitTransformView heroUnit;

        [Inject] private GameModeManager _gameModeManager;
        [Inject] private DiContainer _container;

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
            ecsSystems.Inject(_container.Resolve<Hero>());

            var cityWorldItem = _gameModeManager.PopParameter<CityWorldItem>();
            ecsSystems.Inject(cityWorldItem);
            var city = new Models.CityStructure(cityWorldItem);
            cityView.Init(city);
            ecsSystems.Inject(city);
            ecsSystems.Inject(cityView);

            ecsSystems.Inject(_container.Resolve<ModalMenuManager>());

            ecsSystems.Inject(heroUnit);

            ecsSystems.Inject(_gameModeManager);
        }
    }
}