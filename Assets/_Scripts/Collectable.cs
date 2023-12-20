#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public abstract class Collectable : MonoBehaviour, IInteractable // TODO INTERFACES
    {
        public virtual void Interact(Player player)
        {
            // Implement specific behaviour. 

            Destroy(gameObject);
        }

        public bool IsInteractionEnabled()
        {
            return true;
        }
    }
}