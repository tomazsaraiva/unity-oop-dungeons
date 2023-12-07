#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class WeaponCollider : MonoBehaviour
    {
        #region Variables

        public delegate void OnHitDetected(IHitable hitable);
        public OnHitDetected HitDetected;

        public bool IsEnabled // ENCAPSULATION
        {
            get { return _collider.enabled; }
            set { _collider.enabled = value; }
        }

        private Collider2D _collider;

        #endregion

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IHitable hitable))
            {
                HitDetected?.Invoke(hitable);
            }
        }
    }
}