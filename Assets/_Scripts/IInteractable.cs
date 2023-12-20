namespace OOPDungeons
{
    public interface IInteractable
    {
        void Interact(Player player);
        bool IsInteractionEnabled();
    }
}
