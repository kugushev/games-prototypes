﻿using Kugushev.Scripts.App.ValueObjects;
using Kugushev.Scripts.Common.Modes;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine.Parameterized;
using Zenject;

namespace Kugushev.Scripts.App.Modes
{
    public class AppModeManager : AbstractModeManager
    {
        [Inject] private MainMenuState _mainMenu = default!;

        [Inject] private GameModeState _gameMode = default!;
        [Inject] private SignaledTransition<GameModeParameters> _toGameMode = default!;

        protected override Transitions ComposeStateMachine()
        {
            return new Transitions
            {
                {
                    Entry, new[]
                    {
                        Immediate.TransitTo(_mainMenu)
                    }
                },
                {
                    _mainMenu, new[]
                    {
                        _toGameMode.TransitTo(_gameMode)
                    }
                }
            };
        }
    }
}