using Kugushev.Scripts.Game.Core.ValueObjects;
using Kugushev.Scripts.Game.Politics.Factories;
using Kugushev.Scripts.Game.Politics.Interfaces;
using Kugushev.Scripts.Game.Politics.PresentationModels;
using Kugushev.Scripts.Game.Politics.Widgets;
using Zenject;

namespace Kugushev.Scripts.Game.Politics
{
    public class PoliticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPoliticianSelector>().To<ParliamentPresentationModel>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IIntriguesSelector>().To<IntriguesPresentationModel>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<IntrigueCard, IIntriguesPresentationModel, IntrigueCardPresentationModel,
                    IntrigueCardPresentationModel.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<IntrigueCardFactory>().FromComponentInHierarchy().AsSingle()));
        }
    }
}