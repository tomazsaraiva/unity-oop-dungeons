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
        [SerializeField] private Collectable _collectable;

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
            if (_collectable == null) { return; }

            Instantiate(_collectable, transform.position, _collectable.transform.rotation);
        }
    }
}