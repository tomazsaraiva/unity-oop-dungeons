#region Includes
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;
#endregion

public class Map : MonoBehaviour
{
    [Header("Children")]
    [SerializeField] private Tilemap _background;
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _fog;

    public Bounds Bounds { get; private set; } // ENCAPSULATION

    private Grid _grid;

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
}
