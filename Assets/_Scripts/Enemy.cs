#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public abstract class Enemy : MonoBehaviour, IHitable
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Map _map;
        [SerializeField] private HealthBar _healthBar;

        [Header("Configuration")]
        [SerializeField] private float _hitPoints;
        [SerializeField] private float _idleRange;

        private Vector3 _initialPosition;
        private EnemyState _currentState;
        private float _currentHitPoints;

        #endregion

        protected virtual void Awake()
        {
            _initialPosition = transform.position;
            _currentHitPoints = _hitPoints;
        }

        public void Hit(float amount)
        {
            _currentHitPoints--;
            _healthBar.Decrease(1 / _hitPoints);

            if (_currentHitPoints == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}