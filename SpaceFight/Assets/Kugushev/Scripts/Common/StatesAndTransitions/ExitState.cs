using System;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.Common.StatesAndTransitions
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + nameof(ExitState))]
    public class ExitState : ScriptableObject, IUnparameterizedState, ITransition
    {
        [NonSerialized] private bool _entered;

        public UniTask OnEnterAsync()
        {
            _entered = true;
            return UniTask.CompletedTask;
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public UniTask OnExitAsync()
        {
            _entered = false;
            return UniTask.CompletedTask;
        }

        public bool ToTransition => _entered;
    }
}