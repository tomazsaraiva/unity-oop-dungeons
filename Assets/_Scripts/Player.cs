#region Includes
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

namespace OOPDungeons
{
    public class Player : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Map _map;
        [SerializeField] private Transform _sprite;
        [SerializeField] private WeaponAnchor _weaponAnchor;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Crosshair _crosshair;

        [Header("Configuration")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private InputActionReference _inputUp;
        [SerializeField] private InputActionReference _inputDown;
        [SerializeField] private InputActionReference _inputLeft;
        [SerializeField] private InputActionReference _inputRight;
        [SerializeField] private InputActionReference _inputAttack;
        [SerializeField] private InputActionReference _inputAction;

        public Weapon Weapon { get { return _weapon; } } // ENCAPSULATION

        private float _currentHealth;

        #endregion

        private void Start()
        {
            _inputUp.action.performed += (obj) => MovePlayer(Vector2.up);
            _inputDown.action.performed += (obj) => MovePlayer(Vector2.down);
            _inputLeft.action.performed += (obj) => MovePlayer(Vector2.left);
            _inputRight.action.performed += (obj) => MovePlayer(Vector2.right);
            _inputAttack.action.performed += (obj) => Attack();
            _inputAction.action.performed += (obj) => Action();

            _crosshair.PositionChanged += Crosshair_PositionChanged;

            _currentHealth = _maxHealth;

            MovePlayer(Vector2.zero);
        }

        public void Heal(float amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            // TODO update health bar
        }
        public void Equip(Weapon weapon)
        {
            _weaponAnchor.Assign(weapon.transform);

            if (_weapon != null)
            {
                Destroy(_weapon.gameObject);
            }

            _weapon = weapon;
        }

        private void MovePlayer(Vector2 direction)
        {
            var targetPosition = transform.position + (Vector3)direction;
            if (!_map.CanMoveTo(targetPosition)) { return; }

            transform.position = targetPosition;

            _crosshair.UpdatePlayerPosition(transform.position);
        }
        private void Attack()
        {
            if (_weapon == null) { return; }
            _weapon.Attack();
        }
        private void Action()
        {
            _crosshair.Interact(this);
        }

        private void Crosshair_PositionChanged(Vector3 position)
        {
            var direction = Vector2.right * Mathf.Sign(position.x - transform.position.x);
            _sprite.transform.localScale = new Vector3(direction.x, 1, 1);
            _weaponAnchor.ChangeDirection(direction);
        }
    }
}