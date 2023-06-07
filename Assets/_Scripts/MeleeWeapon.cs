#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class MeleeWeapon : Weapon
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

        protected override void OnAttackStarted()
        {
            base.OnAttackStarted();

            _collider.IsEnabled = true;
        }

        protected override void OnAttackEnded()
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
