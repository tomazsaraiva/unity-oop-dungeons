#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public abstract class Enemy : MonoBehaviour, IHitable
    {
        #region Variables

        [Header("References")]
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Collectable _collectable;

        [Header("Configuration")]
        [SerializeField] private float _hitPoints;
        [SerializeField] private float _idleSpeed;
        [SerializeField] private float _idleRange;
        [SerializeField] private float _detectionRange;
        [SerializeField] private float _attackRange;

        private Map _map;

        private Vector3 _initialPosition;
        private Vector3 _targetPosition;
        private float _currentHitPoints;

        private EnemyState _currentState = EnemyState.Idle;
        private Collider2D _player;

        #endregion

        protected virtual void Awake()
        {
            _initialPosition = transform.position;
            _currentHitPoints = _hitPoints;
        }
        protected virtual void Start()
        {
            _map = FindObjectOfType<Map>();
            _targetPosition = transform.position;
        }
        protected virtual void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Idle:
                    IdleUpdate();
                    break;
                case EnemyState.Chase:
                    ChaseUpdate();

                    break;
                case EnemyState.Attack:
                    AttackUpdate();
                    break;
            }

            if (IsPlayerWithinAttackRange())
            {
                _currentState = EnemyState.Attack;
            }
            else if (IsPlayerWithinDetectionRange())
            {
                _currentState = EnemyState.Chase;
            }
            else
            {
                _currentState = EnemyState.Idle;
            }
        }

        protected virtual bool IsPlayerWithinDetectionRange()
        {
            _player = Physics2D.OverlapCircle(transform.position,
                                              _detectionRange,
                                              LayerMask.GetMask("Player"));
            return _player != null;
        }
        protected virtual bool IsPlayerWithinAttackRange()
        {
            return _player != null &&
                   Vector2.Distance(_player.transform.position, transform.position) < _attackRange;
        }

        protected virtual void IdleUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                     _targetPosition,
                                                     Time.deltaTime * _idleSpeed);

            if (transform.position == _targetPosition)
            {
                _targetPosition = _map.GetRandomTileWithinRange(_initialPosition, (int)_idleRange);
            }
        }
        protected virtual void ChaseUpdate()
        {

        }
        protected virtual void AttackUpdate()
        {

        }

        public virtual void Hit(float amount)
        {
            _currentHitPoints--;
            _healthBar.Decrease(1 / _hitPoints);

            if (_currentHitPoints == 0)
            {
                if (_collectable != null)
                {
                    Instantiate(_collectable, transform.position, _collectable.transform.rotation);
                }

                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _idleRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
#endif
    }
}