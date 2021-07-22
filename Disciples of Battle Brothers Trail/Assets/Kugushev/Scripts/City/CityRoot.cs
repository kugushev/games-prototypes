using Kugushev.Scripts.City.Views;
using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Game.Models;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.City
{
    public class CityRoot : BaseRoot
    {
        [SerializeField] private CityView cityView;
        
        protected override void InitSystems(EcsSystems ecsSystems)
        {
            
        }

        protected override void Inject(EcsSystems ecsSystems)
        {
            ecsSystems.Inject(Hero.Instance);

            var city = new Models.City();
            cityView.Init(city);
            ecsSystems.Inject(city);
            ecsSystems.Inject(cityView);
        }
    }
}