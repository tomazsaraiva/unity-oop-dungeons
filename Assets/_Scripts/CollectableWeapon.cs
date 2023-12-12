#region Includes
using OOPDungeons;
using UnityEngine;
#endregion

public class CollectableWeapon : Collectable
{
    #region Variables

    [Header("References")]
    [SerializeField] private Weapon _weapon;

    #endregion

    public override void Interact(Player player)
    {
        player.Equip(Instantiate(_weapon));

        base.Interact(player);
    }
}
