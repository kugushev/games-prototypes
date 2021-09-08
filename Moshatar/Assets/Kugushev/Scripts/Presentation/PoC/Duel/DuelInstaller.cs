using Kugushev.Scripts.Presentation.PoC.Common;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class DuelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<HeroHeadController>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<Vector3, Vector3, float, BigProjectile, BigProjectile.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f =>
                        f.To<BigProjectilePrefabFactory>().FromComponentInHierarchy().AsSingle()));

            Container.BindFactory<Vector3, Vector3, float, SmallProjectile, SmallProjectile.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f =>
                        f.To<SmallProjectilePrefabFactory>().FromComponentInHierarchy().AsSingle()));

            Container.Bind<FireHeart>().FromComponentInHierarchy().AsSingle();
        }
    }
}