#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Prop : MonoBehaviour, IHitable // TODO INTERFACES
    {
        #region Variables

        [SerializeField] private Collectable _collectible;

        #endregion

        public void Hit(float amount) // TODO POLYMORPHISM
        {
            if (_collectible != null)
            {
                Instantiate(_collectible, transform.position, _collectible.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}