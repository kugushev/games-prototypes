using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameModeManager>().AsSingle();
            Container.Bind<Hero>().AsSingle();
        }
    }
}