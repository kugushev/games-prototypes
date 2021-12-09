using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Presentation.Presenters.Units;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Battle.Presentation
{
    public class BattlePresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Vector3, EnemyFighter, EnemyUnitPresenter, EnemyUnitPresenter.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f =>
                        f.To<EnemyUnitPrefabFactory>().FromComponentInHierarchy().AsSingle()));
        }
    }
}