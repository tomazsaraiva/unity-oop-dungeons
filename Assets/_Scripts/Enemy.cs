#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public abstract class Enemy : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Map _map;

        [Header("Configuration")]
        [SerializeField] private float _idleRange;

        private Vector3 _initialPosition;
        private EnemyState _currentState;

        #endregion

        private void Awake()
        {
            _initialPosition = transform.position;
        }
        private void Start()
        {

        }
        private void Update()
        {

        }
    }
}