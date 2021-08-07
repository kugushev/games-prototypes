﻿using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Presentation.PoC;
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

            Container.BindFactory<Vector3, ZombieView, ZombieView.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.FromIFactory(f => f.To<ZombieViewFactory>().FromComponentInHierarchy().AsSingle()));

            Container.Bind<HitsManager>().AsSingle();
        }
    }
}