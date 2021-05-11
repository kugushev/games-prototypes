using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Tests.Integration.Game.Setup
{
    public class PoliticsTestingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameState>().AsSingle();
        }
    }
}