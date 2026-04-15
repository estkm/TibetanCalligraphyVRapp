using UnityEngine;

public class SymbolTile : MonoBehaviour
{
    [SerializeField] private string symbol;

    public string Symbol => symbol;

    private TileSlot currentSlot;

    public bool IsPlaced => currentSlot != null;

    public void SetCurrentSlot(TileSlot slot)
    {
        currentSlot = slot;
    }

    public void ClearCurrentSlot()
    {
        currentSlot = null;
    }
}
