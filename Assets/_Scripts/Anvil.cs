#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Anvil : MonoBehaviour, IInteractable
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