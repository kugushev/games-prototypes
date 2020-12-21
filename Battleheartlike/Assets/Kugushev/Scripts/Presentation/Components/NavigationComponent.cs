using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Behaviors;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using UnityEngine;
using UnityEngine.AI;

namespace Kugushev.Scripts.Presentation.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavigationComponent : BaseComponent<IMovable>, IPathfindingService
    {
        private NavMeshAgent _navMeshAgent;
        private NavMeshPath _navMeshPathForTest;

        protected override void OnAwake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshPathForTest = new NavMeshPath();
        }

        private void Update()
        {
            if (Model.Destination != null)
            {
                _navMeshAgent.destination = Model.Destination.Value.Point;
            }
        }

        public bool TestDestination(in Position target)
        {
            _navMeshAgent.CalculatePath(target.Point, _navMeshPathForTest);
            return _navMeshPathForTest.status == NavMeshPathStatus.PathComplete;
        }
    }
}