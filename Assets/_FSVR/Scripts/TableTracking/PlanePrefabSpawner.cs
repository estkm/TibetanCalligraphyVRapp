using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlanePrefabSpawner : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private PlaneClassifications targetClassification = PlaneClassifications.Table;
    [SerializeField] private GameObject prefabToSpawn;

    [Tooltip("Raised once, right after the first matching plane is found and prefabToSpawn is instantiated.")]
    [SerializeField] private UnityEvent onTableFound;

    private GameObject spawnedObject;
    private ARPlane selectedPlane;

    // Exposes the placed table so other scripts (e.g. the Play button) can
    // read its final, player-adjusted transform once it's been grabbed into place.
    public Transform SpawnedTransform => spawnedObject != null ? spawnedObject.transform : null;

    private readonly Dictionary<TrackableId, GameObject> spawnedObjects = new();

    private void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(OnTrackablesChanged);

        foreach (var plane in planeManager.trackables)
            TrySpawn(plane);
    }

    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        foreach (var plane in args.added)
            TrySpawn(plane);

        foreach (var plane in args.updated)
            TrySpawn(plane);
    }
    
    private void TrySpawn(ARPlane plane)
    {
        // Already spawned on a table
        if (spawnedObject != null)
            return;

        bool matches = (plane.classifications & targetClassification) != 0;

        if (!matches)
            return;

        selectedPlane = plane;

        spawnedObject = Instantiate(
            prefabToSpawn,
            plane.transform.position,
            plane.transform.rotation);

        // Optional
        // spawnedObject.transform.SetParent(plane.transform);

        onTableFound?.Invoke();
    }
}