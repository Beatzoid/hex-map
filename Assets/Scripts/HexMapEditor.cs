using UnityEngine;
using UnityEngine.EventSystems;

public enum OptionalToggle
{
    Ignore, Yes, No
}

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;

    private Color activeColor;
    private int activeElevation;

    private bool applyColor;
    private bool applyElevation = true;

    private int brushSize;

    private OptionalToggle riverMode;

    private bool isDrag;
    private HexDirection dragDirection;
    private HexCell previousCell;

    public void Awake()
    {
        SelectColor(0);
    }

    public void Update()
    {
        if (
            Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
        else previousCell = null;
    }

    /// <summary>
    /// Select a color with the given index
    /// </summary>
    public void SelectColor(int index)
    {
        applyColor = index >= 0;
        if (applyColor) activeColor = colors[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
        Debug.Log(activeElevation);
    }

    public void SetApplyElevation(bool toggle)
    {
        applyElevation = toggle;
    }

    public void SetBrushSize(float size)
    {
        brushSize = (int)size;
    }

    public void ShowUI(bool visible)
    {
        hexGrid.ShowUI(visible);
    }

    public void SetRiverMode(int mode)
    {
        riverMode = (OptionalToggle)mode;
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main!.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            HexCell currentCell = hexGrid.GetCell(hit.point);

            if (previousCell && previousCell != currentCell)
                ValidateDrag(currentCell);
            else
                isDrag = false;

            EditCells(currentCell);
            previousCell = currentCell;
        }
        else previousCell = null;
    }

    private void ValidateDrag(HexCell currentCell)
    {
        for (
            dragDirection = HexDirection.NE;
            dragDirection <= HexDirection.NW;
            dragDirection++
        )
        {
            if (previousCell.GetNeighbor(dragDirection) == currentCell)
            {
                isDrag = true;
                return;
            }
        }

        isDrag = false;
    }

    private void EditCells(HexCell center)
    {
        int centerX = center.coordinates.X;
        int centerZ = center.coordinates.Z;

        for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + brushSize; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }

        for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
            for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
    }

    private void EditCell(HexCell cell)
    {
        if (cell)
        {
            if (applyColor) cell.Color = activeColor;
            if (applyElevation) cell.Elevation = activeElevation;

            if (riverMode == OptionalToggle.No)
                cell.RemoveRiver();
            else if (isDrag && riverMode == OptionalToggle.Yes)
            {
                HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());

                if (otherCell)
                    otherCell.SetOutgoingRiver(dragDirection);
            }
        }
    }
}
