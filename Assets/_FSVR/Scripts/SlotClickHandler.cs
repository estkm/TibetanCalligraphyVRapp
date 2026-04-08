using FSVR;
using UnityEngine;

public class SlotClickHandler : MonoBehaviour
{
    [SerializeField] private TileSlot slot;
    [SerializeField] private ScrabbleManager puzzleManager;

    private void Reset()
    {
        slot = GetComponent<TileSlot>();
    }

    private void OnMouseDown()
    {
        if (!TileSelector.SelectedTile) return;

        var selected = TileSelector.SelectedTile;

        if (selected.IsPlaced)
            return;

        bool placed = slot.TryPlaceTile(selected);

        if (!placed) return;
        
        TileSelector.SelectedTile = null;
        puzzleManager.CheckWord();
    }
}