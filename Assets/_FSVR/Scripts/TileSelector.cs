using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public static SymbolTile SelectedTile { get;  set; }

    private SymbolTile tile;

    private void Awake()
    {
        tile = GetComponent<SymbolTile>();
    }

    private void OnMouseDown()
    {
        SelectedTile = tile;
        Debug.Log("Selected tile: " + tile.Symbol);
    }
}