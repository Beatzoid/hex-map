using UnityEngine;

public class HexCell : MonoBehaviour
{
    [SerializeField] private HexCell[] neighbors;

    public HexCoordinates coordinates;
    public Color color;

    /// <summary>
    /// Get the neighbor of a cell in the given direction
    /// </summary> 
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    /// <summary>
    /// Set the neighbor of a cell in the given direction to a given cell
    /// </summary>
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
}
