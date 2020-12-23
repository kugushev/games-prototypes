using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using UnityEngine;
using UnityEngine.AI;

namespace Kugushev.Scripts.Presentation.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavigationComponent : BaseComponent<IMovable>, INavigationComponent
    {
        private NavMeshAgent _navMeshAgent;

        protected override void OnAwake() => _navMeshAgent = GetComponent<NavMeshAgent>();

        public bool TrySetDestination(in Position target) => _navMeshAgent.SetDestination(target.Point);

        public bool TestIfDestinationReached() =>
            !_navMeshAgent.pathPending
            && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance
            && (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
    }
}