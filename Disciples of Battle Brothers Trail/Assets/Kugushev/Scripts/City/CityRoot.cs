using Kugushev.Scripts.Common.Ecs;
using Kugushev.Scripts.Game.Models;
using Leopotam.Ecs;

namespace Kugushev.Scripts.City
{
    public class CityRoot : BaseRoot
    {
        protected override void InitSystems(EcsSystems ecsSystems)
        {
            
        }

        protected override void Inject(EcsSystems ecsSystems)
        {
            ecsSystems.Inject(Hero.Instance);
        }
    }
}