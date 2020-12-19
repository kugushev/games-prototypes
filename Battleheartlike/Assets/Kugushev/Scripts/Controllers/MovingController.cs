using Kugushev.Scripts.Models.Behaviors;
using Kugushev.Scripts.Models.Characters.Abstractions;
using Kugushev.Scripts.Models.Services;
using Kugushev.Scripts.ValueObjects;
using UnityEngine;
using UnityEngine.AI;

namespace Kugushev.Scripts.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovingController: MonoBehaviour, IPathfindingService
    {
        [SerializeField] private Character character;
        private NavMeshAgent _navMeshAgent;
        private NavMeshPath _navMeshPathForTest;
        private IMovable Movable => character;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshPathForTest = new NavMeshPath();

            character.PathfindingService = this;
        }

        private void Update()
        {
            if(Movable.Destination != null)
                _navMeshAgent.SetDestination(Movable.Destination.Value.Point);

            // if (!_navMeshAgent.pathPending)
            //     if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            //         if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            //             _animator.SetBool(IsWalking, false);
        }

        public bool TestDestination(in Position target)
        {
            _navMeshAgent.CalculatePath(target.Point, _navMeshPathForTest);
            return _navMeshPathForTest.status == NavMeshPathStatus.PathComplete;
        }
    }
}