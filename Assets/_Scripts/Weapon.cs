#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Variables

        private const string ANIMATION_TRIGGER_ATTACK = "Attack";

        [Header("References")]
        [SerializeField] private HealthBar _healthBar;

        [Header("Configuration")]
        [SerializeField] protected float _attackPower;
        [SerializeField] protected float _maxDamage;
        [SerializeField] protected float _damageIncrement;

        private Animator _animator;
        protected float _currentDamage;

        #endregion

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public virtual void Attack()
        {
            _animator.SetTrigger(ANIMATION_TRIGGER_ATTACK);
        }
        public virtual void Repair()
        {
            _currentDamage = 0;
            _healthBar.Fill();
        }

        protected void Damage()
        {
            if (_currentDamage == _maxDamage) { return; }

            _currentDamage += _damageIncrement;
            _healthBar.Decrease(_damageIncrement / _maxDamage);

            if (_currentDamage > _maxDamage)
            {
                // TODO lose weapon.
            }
        }

        protected void SetDamageLevel()
        {

        }

        protected virtual void OnAttackStarted()
        {
            // Implement specific behaviour.
        }

        protected virtual void OnAttackEnded()
        {
            // Implement specific behaviour.
        }
    }
}