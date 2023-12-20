#region Includes
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#endregion

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Map : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private MapTile _mapTilePrefab;
    [SerializeField] private Transform _tilesParent;

    [Header("Children")]
    [SerializeField] private Tilemap _background;
    [SerializeField] private Tilemap _floor;
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _fog;
    [SerializeField] private MapTile[,] _tiles;

    public Bounds Bounds { get; private set; } // TODO ENCAPSULATION

    private Grid _grid;

    #endregion

    private void Awake()
    {
        _grid = GetComponent<Grid>();

        var localBounds = _background.localBounds;

        var bounds = new Bounds
        {
            center = transform.TransformPoint(localBounds.center),
            size = localBounds.size
        };

        Bounds = bounds;

        GenerateTiles();
    }

    public MapTile GetRandomTileWithinRange(Vector3 position, int range)
    {
        var center = Vector3Int.FloorToInt(_grid.WorldToCell(position));

        var min = new Vector3Int((center - Vector3Int.right * range).x, (center - Vector3Int.up * range).y);
        var max = new Vector3Int((center + Vector3Int.right * range).x, (center + Vector3Int.up * range).y);

        var tiles = new List<Vector3Int>();
        for (int i_x = min.x; i_x <= max.x; i_x++)
        {
            for (int i_y = min.y; i_y <= max.y; i_y++)
            {
                var tile = new Vector3Int(i_x, i_y);
                if (!CanMoveTo(tile)) continue;
                tiles.Add(tile);
            }
        }

        return GetMapTileByCellPosition(tiles[Random.Range(0, tiles.Count)]);
    }
    public MapTile GetMapTileByWorldPosition(Vector3 worldPosition)
    {
        return GetMapTileByCellPosition(_floor.WorldToCell(worldPosition));
    }

    // TODO ABSTRACTION
    public List<MapTile> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        var startTile = GetMapTileByWorldPosition(startPosition);
        var targetTile = GetMapTileByWorldPosition(endPosition);

        var openSet = new List<MapTile> { startTile };
        var closedSet = new HashSet<MapTile>();

        while (openSet.Count > 0)
        {
            var currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentTile.fCost ||
                    openSet[i].fCost == currentTile.fCost &&
                    openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile) break;

            foreach (var neighbour in currentTile.neighbours)
            {
                var newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetTile);
                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return RetracePath(startTile, targetTile);
    }

    public void Clear(Vector3 position)
    {
        var positionInt = Vector3Int.FloorToInt(_grid.WorldToCell(position));

        _fog.SetTile(positionInt + Vector3Int.left, null);
        _fog.SetTile(positionInt, null);
        _fog.SetTile(positionInt + Vector3Int.right, null);

        _fog.SetTile(positionInt + Vector3Int.up, null);
        _fog.SetTile(positionInt + Vector3Int.up + Vector3Int.left, null);
        _fog.SetTile(positionInt + Vector3Int.up + Vector3Int.right, null);

        _fog.SetTile(positionInt + Vector3Int.down, null);
        _fog.SetTile(positionInt + Vector3Int.down + Vector3Int.left, null);
        _fog.SetTile(positionInt + Vector3Int.down + Vector3Int.right, null);
    }
    public bool CanMoveTo(Vector3 position)
    {
        return !_walls.HasTile(_grid.WorldToCell(position));
    }

    private MapTile GetMapTileByCellPosition(Vector3Int position)
    {
        for (int x = 0; x < _floor.size.x; x++)
        {
            for (int y = 0; y < _floor.size.y; y++)
            {
                if (_tiles[x, y] == null) continue;

                var tile = _tiles[x, y];
                if (tile.cellX == position.x && tile.cellY == position.y)
                {
                    return tile;
                }
            }
        }

        return null;
    }
    private int GetDistance(MapTile nodeA, MapTile nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    private List<MapTile> RetracePath(MapTile startNode, MapTile targetNode)
    {
        var path = new List<MapTile>();
        var currentTile = targetNode;

        while (currentTile != startNode)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
        return path;
    }

    private bool CanMoveTo(Vector3Int tile)
    {
        return !_walls.HasTile(tile);
    }

    private void GenerateTiles()
    {
        ClearTiles();

        var size = _floor.size;

        _tiles = new MapTile[size.x, size.y];
        for (int i_X = 0; i_X < size.x; i_X++)
        {
            for (int i_Y = 0; i_Y < size.y; i_Y++)
            {
                var position = new Vector3Int(_floor.origin.x + i_X, _floor.origin.y + i_Y);
                if (!_floor.HasTile(position)) continue;

                var cellPosition = _floor.GetCellCenterWorld(position);

                var tile = (MapTile)PrefabUtility.InstantiatePrefab(_mapTilePrefab, _tilesParent);
                tile.gridX = i_X;
                tile.gridY = i_Y;
                tile.cellX = position.x;
                tile.cellY = position.y;
                tile.transform.position = cellPosition;
                tile.transform.SetParent(_tilesParent);

                _tiles[i_X, i_Y] = tile;
            }
        }

        foreach (MapTile tile in _tiles)
        {
            if (tile == null) continue;

            var x = tile.gridX;
            var y = tile.gridY;

            if (WithinBounds(_tiles, x - 1, y)) tile.AddNeighbour(_tiles[x - 1, y]); // LEFT
            if (WithinBounds(_tiles, x + 1, y)) tile.AddNeighbour(_tiles[x + 1, y]); // RIGHT
            if (WithinBounds(_tiles, x, y + 1)) tile.AddNeighbour(_tiles[x, y + 1]); // TOP
            if (WithinBounds(_tiles, x, y - 1)) tile.AddNeighbour(_tiles[x, y - 1]); // BOTTOM

            if (WithinBounds(_tiles, x - 1, y + 1)) tile.AddNeighbour(_tiles[x - 1, y + 1]); // TOP LEFT
            if (WithinBounds(_tiles, x + 1, y + 1)) tile.AddNeighbour(_tiles[x + 1, y + 1]); // TOP RIGHT
            if (WithinBounds(_tiles, x - 1, y - 1)) tile.AddNeighbour(_tiles[x - 1, y - 1]); // BOTTOM LEFT
            if (WithinBounds(_tiles, x + 1, y - 1)) tile.AddNeighbour(_tiles[x + 1, y - 1]); // BOTTOM RIGHT
        }

        EditorUtility.SetDirty(this);
    }
    private bool WithinBounds(MapTile[,] array, int x, int y)
    {
        if (x < array.GetLowerBound(0) ||
            x > array.GetUpperBound(0) ||
            y < array.GetLowerBound(1) ||
            y > array.GetUpperBound(1)) return false;
        return true;
    }
    private void ClearTiles()
    {
        for (int i = _tilesParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_tilesParent.GetChild(i).gameObject);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {
        #region Variables

        private Map _target;

        #endregion

        private void OnEnable()
        {
            _target = (Map)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Generate Tiles"))
            {
                _target.GenerateTiles();
            }
            if (GUILayout.Button("Clear"))
            {
                _target.ClearTiles();
            }
        }
    }
#endif
}
