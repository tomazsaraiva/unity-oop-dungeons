#region Includes
using System;
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
        [SerializeField] private float _attackPower;
        [SerializeField] protected float _idleRange;
        [SerializeField] private float _detectionRange;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _idleSpeed;
        [SerializeField] private float _chaseSpeed;
        [SerializeField] private float _attackSpeed;

        protected EnemyState CurrentState { get { return _currentState; } }
        protected float CurrentSpeed { get { return GetCurrentSpeed(); } }
        protected Collider2D Player { get { return _player; } }

        protected Map _map;
        private EnemyState _currentState = EnemyState.Idle;

        private float _currentHitPoints;
        private Collider2D _player;

        #endregion

        protected virtual void Awake()
        {
            _currentHitPoints = _hitPoints;
        }
        protected virtual void Start()
        {
            _map = FindObjectOfType<Map>();

            ChangeState(EnemyState.Idle);
        }
        protected virtual void Update()
        {
            if (IsPlayerWithinAttackRange())
            {
                if (_currentState != EnemyState.Attack) ChangeState(EnemyState.Attack);
            }
            else if (IsPlayerWithinDetectionRange())
            {
                if (_currentState != EnemyState.Chase) ChangeState(EnemyState.Chase);
            }
            else
            {
                if (_currentState != EnemyState.Idle) ChangeState(EnemyState.Idle);
            }
        }

        protected virtual void ChangeState(EnemyState state)
        {
            _currentState = state;
            Debug.Log("CHANGE STATE " + state);
        }

        private bool IsPlayerWithinDetectionRange()
        {
            _player = Physics2D.OverlapCircle(transform.position,
                                              _detectionRange,
                                              LayerMask.GetMask("Player"));
            return _player != null;
        }
        private bool IsPlayerWithinAttackRange()
        {
            return _player != null &&
                   Vector2.Distance(_player.transform.position, transform.position) < _attackRange;
        }

        public virtual void Hit(float amount) // TODO POLYMORPHISM
        {
            _currentHitPoints--;
            _healthBar.Decrease(amount / _hitPoints);

            if (_currentHitPoints == 0)
            {
                if (_collectable != null)
                {
                    Instantiate(_collectable, transform.position, _collectable.transform.rotation);
                }

                Destroy(gameObject);
            }
        }

        private float GetCurrentSpeed()
        {
            switch (_currentState)
            {
                case EnemyState.Idle: return _idleSpeed;
                case EnemyState.Chase: return _chaseSpeed;
                case EnemyState.Attack: return _attackSpeed;
                default: throw new NotImplementedException(_currentState + " not implemented.");
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player player))
            {
                player.Hit(_attackPower);
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