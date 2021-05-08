using System;
using Kugushev.Scripts.App.Core.ContextManagement;
using Kugushev.Scripts.App.Core.ValueObjects;
using Kugushev.Scripts.Common.ContextManagement;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Kugushev.Scripts.Game.Core.Signals;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Tests.Integration.Game.Setup
{
    public class TestContextManager : AbstractContextManager
    {
        [SerializeField] private Intrigue intrigue = default!;

        [Inject] private GameState _gameState = default!;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private IntrigueCardObtained.Factory _intrigueCardObtainedFactory = default!;

        protected override Transitions ComposeStateMachine()
        {
            return new Transitions
            {
                {
                    Entry, new[]
                    {
                        SingletonTransition<GameParameters>.Instance.TransitTo(_gameState)
                    }
                }
            };
        }

        private void FixedUpdate()
        {
            _signalBus.Fire(_intrigueCardObtainedFactory.Create(intrigue));
        }
    }
}