using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common;
using Zenject;

namespace Kugushev.Scripts.Game
{
    public class GameInstaller : MonoInstaller
    {
        [InjectOptional(Id = CommonConstants.SceneParameters)]
        private GameModeParameters _gameModeParameters = new GameModeParameters(42);

        public override void InstallBindings()
        {
            Container.Bind<GameModeParameters>().FromInstance(_gameModeParameters);

            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}