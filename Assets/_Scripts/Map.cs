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
    [Header("References")]
    [SerializeField] private MapTile _mapTilePrefab;
    [SerializeField] private Transform _tilesParent;

    [Header("Children")]
    [SerializeField] private Tilemap _background;
    [SerializeField] private Tilemap _floor;
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _fog;

    public Bounds Bounds { get; private set; } // ENCAPSULATION

    private Grid _grid;
    private MapTile[,] _tiles;

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
    }

    public Vector3 GetRandomTileWithinRange(Vector3 position, int range)
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

        return _grid.CellToWorld(tiles[Random.Range(0, tiles.Count)]);
    }
    private bool CanMoveTo(Vector3Int tile)
    {
        return !_walls.HasTile(tile);
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

#if UNITY_EDITOR

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
                MapTile tile = null;
                if (_floor.HasTile(position))
                {
                    var cellPosition = _floor.GetCellCenterWorld(position);

                    tile = (MapTile)PrefabUtility.InstantiatePrefab(_mapTilePrefab, _tilesParent);
                    tile.gridX = i_X;
                    tile.gridY = i_Y;
                    tile.transform.position = cellPosition;
                    tile.transform.SetParent(_tilesParent);
                }

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
