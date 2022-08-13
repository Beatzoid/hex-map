using UnityEngine;

public enum HexEdgeType
{
    Flat, Slope, Cliff
}

public class HexCell : MonoBehaviour
{
    [SerializeField] private HexCell[] neighbors;

    public HexCoordinates coordinates;
    public Color color;
    public RectTransform uiRect;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    private int elevation;

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

    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(
            elevation, neighbors[(int)direction].elevation
        );
    }

    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(
            elevation, otherCell.elevation
        );
    }
}
