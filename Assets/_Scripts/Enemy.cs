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
        [SerializeField] private float _idleRange;

        private Map _map;

        private Vector3 _initialPosition;
        private EnemyState _currentState;

        private float _currentHitPoints;

        #endregion

        protected virtual void Awake()
        {
            _initialPosition = transform.position;
            _currentHitPoints = _hitPoints;
        }
        protected virtual void Start()
        {
            _map = FindObjectOfType<Map>();
        }

        public void Hit(float amount)
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
    }
}