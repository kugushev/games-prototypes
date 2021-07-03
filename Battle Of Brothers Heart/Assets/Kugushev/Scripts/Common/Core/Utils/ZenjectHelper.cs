﻿using Zenject;

namespace Kugushev.Scripts.Common.Core.ZenjectUtils
{
    public static class ZenjectHelper
    {
        public static void InstallPrefabPoolFactory<TModel, TParent, TPresentationModel, TPlaceholderFactory,
            TRealFactory>(this DiContainer container)
            where TPlaceholderFactory : PlaceholderFactory<TModel, TParent, TPresentationModel>
            where TRealFactory : IFactory<TPresentationModel>
            where TPresentationModel : UnityEngine.Component, IPoolable<TModel, TParent, IMemoryPool>
        {
            container.BindFactory<TModel, TParent, TPresentationModel,
                    TPlaceholderFactory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<TRealFactory>().FromComponentInHierarchy().AsSingle()));
        }

        public static void InstallPrefabPoolFactory<TModel, TPresentationModel, TPlaceholderFactory,
            TRealFactory>(this DiContainer container)
            where TPlaceholderFactory : PlaceholderFactory<TModel, TPresentationModel>
            where TRealFactory : IFactory<TPresentationModel>
            where TPresentationModel : UnityEngine.Component, IPoolable<TModel, IMemoryPool>
        {
            container.BindFactory<TModel, TPresentationModel,
                    TPlaceholderFactory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<TRealFactory>().FromComponentInHierarchy().AsSingle()));
        }

        public static void InstallPrefabFactory<TModel, TPresentationModel, TPlaceholderFactory,
            TRealFactory>(this DiContainer container)
            where TPlaceholderFactory : PlaceholderFactory<TModel, TPresentationModel>
            where TRealFactory : IFactory<TModel, TPresentationModel>
            where TPresentationModel : UnityEngine.Component
        {
            container.BindFactory<TModel, TPresentationModel, TPlaceholderFactory>()
                .FromIFactory(f => f.To<TRealFactory>().FromComponentInHierarchy().AsSingle());
        }
    }
}