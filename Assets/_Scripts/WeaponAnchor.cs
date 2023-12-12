#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class WeaponAnchor : MonoBehaviour
    {
        #region Variables

        private float _initialPositionX;

        #endregion

        private void Awake()
        {
            _initialPositionX = transform.localPosition.x;
        }

        public void Assign(Transform weapon)
        {
            weapon.SetParent(transform);
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            weapon.localScale = Vector3.one;
        }
        public void ChangeDirection(Vector2 direction)
        {
            var position = transform.localPosition;
            position.x = _initialPositionX * direction.x;
            transform.localPosition = position;
            transform.localScale = new Vector3(direction.x, 1, 1);
        }
    }
}