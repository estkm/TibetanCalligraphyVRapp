using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class SurfaceRecognizer : MonoBehaviour
{
    [SerializeField] private MRUKAnchor.SceneLabels _targetSurfaceLabel = MRUKAnchor.SceneLabels.TABLE;
    [SerializeField] private GameObject _debugCubePrefab;

    private List<GameObject> _debugCubes = new List<GameObject>();

    public void OnSceneLoaded()
    {
        // Retrieve the current room
        var currentRoom = MRUK.Instance.GetCurrentRoom();

        if (currentRoom == null)
        {
            Debug.LogWarning("No current room found!");
            return;
        }

        // Find all surfaces with the label: TABLE 
        var tableSurfaces = new List<MRUKAnchor>();
        foreach (var anchor in currentRoom.Anchors)
        {
            if (anchor.Label == _targetSurfaceLabel)
            {
                tableSurfaces.Add(anchor);
            }
        }

        if (tableSurfaces.Count > 0)
        {
            Debug.Log($"Found {tableSurfaces.Count} surfaces with label: {_targetSurfaceLabel}");

            // Place a debug cube on each surface
            foreach (var surface in tableSurfaces)
            {
                PlaceDebugCubeOnSurface(surface);
            }
        }
        else
        {
            Debug.Log($"No surfaces found with label: {_targetSurfaceLabel}");
        }
    }

    private void PlaceDebugCubeOnSurface(MRUKAnchor surface)
    {
        if (_debugCubePrefab == null)
        {
            Debug.LogError("Debug cube prefab is not assigned!");
            return;
        }

        // Instantiate debug cube
        var debugCube = Instantiate(_debugCubePrefab);
        _debugCubes.Add(debugCube);

        // Calculate surface size and position to place on top of the surface
        var anchorSize = surface.PlaneRect.HasValue ?
            new Vector3(surface.PlaneRect.Value.width, surface.PlaneRect.Value.height, 0.01f) :
            surface.VolumeBounds.Value.size;

        debugCube.transform.localScale = anchorSize + new Vector3(0.01f, 0.01f, 0.01f);

        debugCube.transform.position = surface.PlaneRect.HasValue ?
            surface.transform.position :
            surface.transform.TransformPoint(surface.VolumeBounds.Value.center);

        debugCube.transform.rotation = surface.transform.rotation;

        Debug.Log($"Placed debug cube on surface: {surface.name} Type: {surface.Label} at position: {debugCube.transform.position}");
    }
}


