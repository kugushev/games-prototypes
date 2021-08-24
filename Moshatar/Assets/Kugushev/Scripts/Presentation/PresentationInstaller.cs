using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Presentation.PoC;
using Kugushev.Scripts.Presentation.PoC.Music;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation
{
    public class PresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<string, Vector3, PopupText, PopupText.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<PopupTextFactory>().FromComponentInHierarchy().AsSingle()));

            Container.BindFactory<Vector3, ZombiesSpawner, ZombieView, ZombieView.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<ZombieViewFactory>().FromComponentInHierarchy().AsSingle()));

            Container.BindFactory<Vector3, Vector3, ZombieProjectile, ZombieProjectile.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<ZombieProjectileFactory>().FromComponentInHierarchy().AsSingle()));

            Container.Bind<HitsManager>().AsSingle();
            Container.Bind<Hero>().FromComponentInHierarchy().AsSingle();

            Container.Bind<GameDirector>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ChargeManager>().AsSingle();
        }
    }
}