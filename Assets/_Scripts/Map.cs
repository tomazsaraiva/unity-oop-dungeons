using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [Header("Children")]
    [SerializeField] private Tilemap _background;
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _fog;

    public Bounds Bounds { get;  private set; }

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
