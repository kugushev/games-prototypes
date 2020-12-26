using System;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Game.Services;
using Kugushev.Scripts.Presentation.Common;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using UnityEngine;
using UnityEngine.AI;

namespace Kugushev.Scripts.Presentation.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementComponent : BaseComponent<IMovable>, INavigationService
    {
        [SerializeField] private Animator animator;
        private NavMeshAgent _navMeshAgent;

        protected override void OnAwake() => _navMeshAgent = GetComponent<NavMeshAgent>();

        private void Start()
        {
            _navMeshAgent.updateRotation = false;
        }

        private void Update()
        {
            animator.SetBool(AnimatorParameters.Walking, Model.IsMoving);
        }

        private void LateUpdate()
        {
            // rotate instantly
            if (_navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)
                transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity.normalized);
        }

        public bool TrySetDestination(in Position target) => _navMeshAgent.SetDestination(target.Point);

        public bool TestIfDestinationReached() =>
            !_navMeshAgent.pathPending
            && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance
            && (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
    }
}