using Kugushev.Scripts.App.StatesAndTransitions;
using Zenject;

namespace Kugushev.Scripts.App
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AppManager>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<ToNewGameTransition>().AsSingle();

            Container.Bind<AppSceneLoader>().AsSingle();
        }
    }
}