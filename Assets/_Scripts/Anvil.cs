#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Anvil : MonoBehaviour, IInteractable // TODO INTERFACES
    {
        public void Interact(Player player)
        {
            if (player.Weapon == null) { return; }
            player.Weapon.Repair();
        }
        public bool IsInteractionEnabled()
        {
            return true;
        }
    }
}