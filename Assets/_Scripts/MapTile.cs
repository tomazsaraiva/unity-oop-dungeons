#region Includes
using System.Collections.Generic;
using System.Security;
using UnityEngine;
#endregion

public class MapTile : MonoBehaviour
{
    #region Variables

    public int gCost; // Distance from the starting cell node
    public int hCost; // Distance from ending cell node
    public int gridX;
    public int gridY;
    public List<MapTile> neighbours;
    public MapTile parent;

    public int fCost { get { return gCost + hCost; } }

    #endregion

    public void AddNeighbour(MapTile neighbour)
    {
        if (neighbour == null) return;
        neighbours.Add(neighbour);
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
