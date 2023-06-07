#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Crosshair : MonoBehaviour
    {
        #region Variables

        public delegate void OnPositionValueChanged(Vector3 position);
        public event OnPositionValueChanged PositionChanged;

        [Header("References")]
        [SerializeField] private Map _map;

        private Camera _camera;

        private Vector3 _lastMousePosition;
        private Vector3 _lastPosition;

        private Vector3 _playerPosition;

        #endregion

        private void Awake()
        {
            _camera = Camera.main;
        }
        private void Update()
        {
            if (Input.mousePosition == _lastMousePosition) { return; }
            _lastMousePosition = Input.mousePosition;

            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            var offsetX = mouseWorldPosition.x - _playerPosition.x;
            var offsetY = mouseWorldPosition.y - _playerPosition.y;

            var nextPosition = Vector2.zero;
            nextPosition.x = Mathf.Abs(offsetX) > 0.5f ? Mathf.Sign(offsetX) : 0;
            nextPosition.y = Mathf.Abs(offsetY) > 0.5f ? Mathf.Sign(offsetY) : 0;

            var targetPosition = _playerPosition + (Vector3)nextPosition;
            if (targetPosition == _lastPosition) { return; }
            _lastPosition = targetPosition;

            transform.position = targetPosition;
            PositionChanged?.Invoke(targetPosition);
        }

        public void Interact(Player player)
        {
            var hit = Physics2D.Raycast(transform.position, -Vector2.up);
            if (hit.collider == null) { return; }
            if (!hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable)) { return; }
            if (!interactable.IsInteractionEnabled()) { return; }
            interactable.Interact(player);
        }
        public void UpdatePlayerPosition(Vector3 playerPosition)
        {
            _playerPosition = playerPosition;
        }
    }

}