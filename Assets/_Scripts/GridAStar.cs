#region Includes
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace OOPDungeons
{
    public class Node
    {
        public Vector3Int Location { get; }
        public float H { get; } // cost to goal, ex.distance or for example distance + damage taken
        public float G { get; } // cost from start
        public float F { get; } // total cost, G + H
        public Node Parent { get; }

        public Node(Vector3Int location, float h, float g, float f, Node parent)
        {
            Location = location;
            H = h;
            G = g;
            F = f;
            Parent = parent;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return Location.Equals(((Node)obj).Location);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    public class GridAStar : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Grid _grid;

        private List<Vector3Int> directions = new List<Vector3Int>()
        {
            new Vector3Int(1, 0), // right
            new Vector3Int(0, 1), // up
            new Vector3Int(-1, 0), // left
            new Vector3Int(0, -1), // down
        };

        private List<Node> _open = new List<Node>();
        private List<Node> _closed = new List<Node>();
        private bool _isDone;

        #endregion

        public void Search(Node thisNode)
        {

        }
    }
}