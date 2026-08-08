using UnityEngine;

// Moves the Welcome/Play UI to sit just above the table right after it's placed,
// matching the table's own rotation. Without this the Canvas stays at a fixed spot
// hardcoded in the scene, unrelated to wherever the real table actually got detected.
// Wired to PlanePrefabSpawner's onTableFound event (same event that activates the Canvas).
public class PositionUIAtTable : MonoBehaviour
{
    [SerializeField] private PlanePrefabSpawner spawner;
    [SerializeField] private Transform uiTransform;
    [SerializeField] private float heightAboveTable = 0.5f;

    // Wired to PlanePrefabSpawner.onTableFound.
    public void PositionInFrontOfTable()
    {
        Transform table = spawner.SpawnedTransform;
        if (table == null)
            return;

        uiTransform.position = table.position + Vector3.up * heightAboveTable;
        uiTransform.rotation = table.rotation;
    }
}
