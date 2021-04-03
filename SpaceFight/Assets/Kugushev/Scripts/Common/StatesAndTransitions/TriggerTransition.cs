using System;
using Kugushev.Scripts.Common.Utils.FiniteStateMachine;
using UnityEngine;

namespace Kugushev.Scripts.Common.StatesAndTransitions
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + nameof(TriggerTransition))]
    public class TriggerTransition : ScriptableObject, IReusableTransition
    {
        [NonSerialized] private bool isSet;

        public void Set() => isSet = true;

        bool ITransition.ToTransition => isSet;

        void IReusableTransition.Reset() => isSet = false;
    }
}