using UnityEngine;

public class TileSlot : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;

    private SymbolTile _currentSymbolTile;

    public SymbolTile currentSymbolTile => _currentSymbolTile;
    public bool IsOccupied => _currentSymbolTile != null;

    public bool TryPlaceTile(SymbolTile symbolTile)
    {
        if (symbolTile == null) return false;
        if (IsOccupied) return false;

        _currentSymbolTile = symbolTile;
        _currentSymbolTile.SetCurrentSlot(this);

        var target = snapPoint != null ? snapPoint : transform;
        symbolTile.transform.position = target.position;
        symbolTile.transform.rotation = target.rotation;

        return true;
    }

    public void RemoveTile()
    {
        if (_currentSymbolTile == null) return;

        _currentSymbolTile.ClearCurrentSlot();
        _currentSymbolTile = null;
    }
}