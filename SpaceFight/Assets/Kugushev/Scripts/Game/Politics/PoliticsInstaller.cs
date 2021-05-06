using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.ValueObjects;
using Kugushev.Scripts.Game.Widgets;
using Kugushev.Scripts.Game.Widgets.Factories;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Presentation.Politics
{
    public class PoliticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPoliticianSelector>().To<ParliamentWidget>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IIntriguesSelector>().To<IntriguesPresentationModel>().FromComponentInHierarchy().AsSingle();

            // Container.Bind<IntrigueCardFactory>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<IntrigueRecord, ToggleGroup, IntrigueCardPresentationModel,
                    IntrigueCardPresentationModel.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<IntrigueCardFactory>().FromComponentInHierarchy().AsSingle()));
        }
    }
}