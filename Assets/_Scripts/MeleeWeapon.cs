#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class MeleeWeapon : Weapon  // INHERITANCE
    {
        #region Variables

        [Header("References")]
        [SerializeField] private WeaponCollider _collider;

        #endregion

        protected override void Awake()
        {
            base.Awake();

            _collider.HitDetected = WeaponCollider_HitDetected;
        }

        protected override void OnAttackStarted()  // POLYMORPHISM
        {
            base.OnAttackStarted();

            _collider.IsEnabled = true;
        }

        protected override void OnAttackEnded() // POLYMORPHISM
        {
            base.OnAttackEnded();

            _collider.IsEnabled = false;
        }

        private void WeaponCollider_HitDetected(IHitable hitable)
        {
            hitable.Hit(_attackPower * (0.9f * (_currentDamage / _maxDamage)));

            Damage();
        }
    }
}
