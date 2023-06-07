#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Prop : MonoBehaviour, IHitable
    {
        #region Variables

        [SerializeField] private Collectible _collectible;

        #endregion

        public void Hit(float amount) // ABSTRACTION
        {
            if (_collectible != null)
            {
                Instantiate(_collectible, transform.position, _collectible.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}