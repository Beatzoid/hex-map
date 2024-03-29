using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField] private int x, z;

    public int X => x;

    public int Z => z;

    public int Y => -X - Z;

    /// <summary>
    /// The HexCoordinates class is responsible for managing and calculating coordinates
    /// </summary>
    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    /// <summary>
    /// Get the correct coordinates for a given x and z
    /// </summary>
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - (z / 2), z);
    }

    /// <summary>
    /// Generate a string with the coordinates. Ex: (1, 0, 2)
    /// </summary>
    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    /// <summary>
    /// Generate a string with the coordinates on separate lines
    /// </summary>
    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    /// <summary>
    /// Get the coordinates of a cell given a position
    /// </summary>
    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.outerRadius * 3f);

        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }
}
