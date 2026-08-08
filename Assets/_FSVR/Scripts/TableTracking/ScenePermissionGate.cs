using UnityEngine;
using UnityEngine.XR.ARFoundation;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

// Meta's Scene API (the extensions behind ARPlaneManager on Quest: XR_FB_scene,
// XR_FB_spatial_entity, etc.) refuses to return any data until the app has been
// granted this Android permission. Unity/Meta's OpenXR package auto-adds the
// permission to the built manifest, but does NOT request it at runtime for you -
// that part has to be done in game code, which is what this script does.
// planeManager should start disabled in the scene; this script turns it on only
// after the permission is confirmed, matching Meta's documented setup.
public class ScenePermissionGate : MonoBehaviour
{
    private const string ScenePermission = "com.oculus.permission.USE_SCENE";

    [SerializeField] private ARPlaneManager planeManager;

    private bool planeManagerEnabled;

    private void Start()
    {
        RequestOrEnable();
    }

#if UNITY_ANDROID
    // Covers the case where the user denies the permission, then grants it manually
    // from Android Settings without relaunching the app - re-checks (and re-requests,
    // in case the OS dialog can still show) as soon as the app regains focus.
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && !planeManagerEnabled)
            RequestOrEnable();
    }
#endif

    private void RequestOrEnable()
    {
#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(ScenePermission))
        {
            EnablePlaneManager();
            return;
        }

        var callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += OnPermissionGranted;
        callbacks.PermissionDenied += OnPermissionDenied;
        Permission.RequestUserPermission(ScenePermission, callbacks);
#else
        // No Android permission system in the Editor/XR Simulation - enable directly
        // so table detection still works there.
        EnablePlaneManager();
#endif
    }

#if UNITY_ANDROID
    private void OnPermissionGranted(string permission)
    {
        EnablePlaneManager();
    }

    private void OnPermissionDenied(string permission)
    {
        Debug.LogError($"{ScenePermission} was denied - table detection can't work until it's granted. " +
                        "Will retry automatically next time the app regains focus (e.g. after visiting Android Settings).");
    }
#endif

    private void EnablePlaneManager()
    {
        planeManagerEnabled = true;
        planeManager.enabled = true;
    }
}
