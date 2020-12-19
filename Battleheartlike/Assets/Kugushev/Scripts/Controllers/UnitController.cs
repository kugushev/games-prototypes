using UnityEngine;
using UnityEngine.AI;

namespace Kugushev.Scripts.Controllers
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitController: MonoBehaviour
    {
        [SerializeField] private Transform target;

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SetDestination(target.position);
        }

        public void SetDestination(Vector3 destination)
        {
            _animator.SetBool(IsWalking, true);
            _navMeshAgent.SetDestination(destination);
        }

        public void Grab()
        {
            _navMeshAgent.enabled = false;
            _animator.SetBool(IsWalking, false);
        }

        public void Release()
        {
            _navMeshAgent.enabled = true;
        }

        public void Attack() => _animator.SetTrigger(AttackTrigger);

        private void Update()
        {
            if (!_navMeshAgent.pathPending)
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                        _animator.SetBool(IsWalking, false);
        }
    }
}