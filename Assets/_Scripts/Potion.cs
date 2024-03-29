#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Potion : Collectable // TODO INHERITANCE
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private float amount;

        #endregion

        public override void Interact(Player player)
        {
            player.Heal(amount);

            base.Interact(player);
        }
    }
}