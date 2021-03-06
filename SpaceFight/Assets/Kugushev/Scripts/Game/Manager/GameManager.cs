using System.Collections.Generic;
using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Common.FiniteStateMachine.Tools;
using Kugushev.Scripts.Game.Models;
using UnityEngine;

namespace Kugushev.Scripts.Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameModel model;
        private StateMachine _stateMachine;

        private void Awake() => ComposeStateMachine();

        private void ComposeStateMachine()
        {
            var mainMenuState = new MainMenuState(model);

            _stateMachine = new StateMachine(new Dictionary<IState, IReadOnlyList<TransitionRecord>>
            {
                {EntryState.Instance, new[] {new TransitionRecord(ImmediateTransition.Instance, mainMenuState)}}
            });
        }

        private void Update()
        {
            _stateMachine.UpdateAsync(() => Time.deltaTime);
        }
    }
}