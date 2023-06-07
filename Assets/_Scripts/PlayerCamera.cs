#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _player;
        [SerializeField] private Map _map;

        private Bounds _bounds;
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            var cameraHeight = 2f * _camera.orthographicSize;
            var cameraWidth = cameraHeight * _camera.aspect;

            _bounds = new Bounds();
            _bounds.center = _map.Bounds.center;
            _bounds.min = _map.Bounds.min + new Vector3(cameraWidth / 2, cameraHeight / 2, 0);
            _bounds.max = _map.Bounds.max - new Vector3(cameraWidth / 2, cameraHeight / 2, 0);
        }
        private void Update()
        {
            var targetPosition = _player.position;
            targetPosition.x = Mathf.Clamp(targetPosition.x, _bounds.min.x, _bounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, _bounds.min.y, _bounds.max.y);
            targetPosition.z = transform.position.z;

            transform.position = targetPosition;
        }
    }
}