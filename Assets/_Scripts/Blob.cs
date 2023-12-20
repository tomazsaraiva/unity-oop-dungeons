#region Includes
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Blob : Enemy // TODO INHERITANCE
    {
        #region Variables

        private Vector3 _initialPosition;

        private MapTile _nextTile;
        private MapTile _targetTile;

        private int _pathIndex;

        #endregion

        protected override void Start()
        {
            _initialPosition = transform.position;
            base.Start();
        }
        protected override void Update()
        {
            base.Update();

            if (_nextTile == null) return;

            transform.position = Vector2.MoveTowards(transform.position,
                                                    _nextTile.Position,
                                                    CurrentSpeed * Time.deltaTime);

            if (transform.position == _nextTile.Position)
            {
                if (_nextTile != _targetTile)
                {
                    GetNextTile();
                    return;
                }

                if (CurrentState == EnemyState.Idle)
                {
                    MoveToRandomTile();
                }
            }

            if (CurrentState != EnemyState.Idle)
            {
                MoveToPlayer();
            }
        }

        protected override void ChangeState(EnemyState state)
        {
            base.ChangeState(state);

            switch (state)
            {
                case EnemyState.Idle:
                    MoveToRandomTile();
                    break;
                case EnemyState.Chase:
                case EnemyState.Attack:
                    MoveToPlayer();
                    break;
            }
        }

        private void GetNextTile()
        {
            _nextTile = _nextTile.GetClosestNeighbour(_targetTile.Position);
        }
        private void MoveToRandomTile()
        {
            _nextTile = _map.GetMapTileByWorldPosition(transform.position);
            _targetTile = _map.GetRandomTileWithinRange(_initialPosition, (int)_idleRange);
        }
        private void MoveToPlayer()
        {
            _targetTile = _map.GetMapTileByWorldPosition(Player.transform.position);
        }
    }
}