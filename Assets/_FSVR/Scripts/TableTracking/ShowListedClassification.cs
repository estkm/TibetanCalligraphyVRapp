
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ShowListedClassification : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public PlaneClassifications classificationToShow;

    private void OnEnable()
    {
        if (planeManager != null)
            planeManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        if (planeManager != null)
            planeManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        foreach (var plane in args.added)
            UpdatePlane(plane);

        foreach (var plane in args.updated)
            UpdatePlane(plane);
    }

    private void UpdatePlane(ARPlane plane)
    {
        bool matches = (plane.classifications & classificationToShow) != 0;
        plane.gameObject.SetActive(matches);
    }
}