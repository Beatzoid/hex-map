using UnityEngine;

public enum HexEdgeType
{
    Flat, Slope, Cliff
}

public class HexCell : MonoBehaviour
{
    [SerializeField] private HexCell[] neighbors;

    public HexCoordinates coordinates;
    public RectTransform uiRect;
    public HexGridChunk chunk;

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if (color == value) return;
            color = value;
            Refresh();
        }
    }

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            if (elevation == value) return;
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y +=
                ((HexMetrics.SampleNoise(position).y * 2f) - 1f) *
                HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;

            Refresh();
        }
    }

    private int elevation = int.MinValue;
    private Color color;

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

    private void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();

            for (int i = 0; i < neighbors.Length; i++)
            {
                HexCell neighbor = neighbors[i];

                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }
}
