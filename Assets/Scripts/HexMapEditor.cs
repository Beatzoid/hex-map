using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    private Color activeColor;

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
    }

    /// <summary>
    /// Select a color with the given index
    /// </summary>
    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main!.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            hexGrid.ColorCell(hit.point, activeColor);
        }
    }
}
