#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Chest : MonoBehaviour, IInteractable
    {
        #region Variables

        private const string ANIMATION_TRIGGER_OPEN = "Open";

        [Header("References")]
        [SerializeField] private Collectable _collectible;

        private Animator _animator;
        private bool _isOpened;

        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Interact(Player player) // ABSTRACTION
        {
            if (_isOpened) { return; }
            _isOpened = true;

            _animator.SetTrigger(ANIMATION_TRIGGER_OPEN);
        }
        public bool IsInteractionEnabled()
        {
            return !_isOpened;
        }

        private void OnAnimationEnd()
        {
            if (_collectible == null) { return; }

            Instantiate(_collectible, transform.position, _collectible.transform.rotation);
        }
    }
}