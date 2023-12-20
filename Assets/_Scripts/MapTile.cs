#region Includes
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion

public class MapTile : MonoBehaviour
{
    #region Variables

    public int gCost; // Distance from the starting cell node
    public int hCost; // Distance from ending cell node
    public int gridX;
    public int gridY;
    public int cellX;
    public int cellY;
    public List<MapTile> neighbours;
    public MapTile parent;

    public int fCost { get { return gCost + hCost; } }
    public Vector3 Position { get { return transform.position; } }

    #endregion

    public void AddNeighbour(MapTile neighbour)
    {
        if (neighbour == null) return;
        neighbours.Add(neighbour);
    }
    public MapTile GetClosestNeighbour(Vector3 position)
    {
        var minDistance = -1f;
        var index = 0;
        for (int i = 0; i < neighbours.Count; i++)
        {
            var distance = Vector3.Distance(position, neighbours[i].Position);
            if (minDistance == -1 || distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }
        return neighbours[index];
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.25f);

        Gizmos.color = Color.green;
        for (int i = 0; i < neighbours.Count; i++)
        {
            var neighbourPosition = neighbours[i].transform.position;
            Gizmos.DrawWireSphere(neighbourPosition, 0.1f);
            Gizmos.DrawLine(transform.position, neighbourPosition);
        }
    }
#endif
}
