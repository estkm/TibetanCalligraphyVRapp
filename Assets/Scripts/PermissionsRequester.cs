using UnityEngine;

public class MetaPermissionRequester : MonoBehaviour
{
    void Denied(string permission) => Debug.Log($"{permission} Denied");
    void Granted(string permission) => Debug.Log($"{permission} Granted");

    public void RequestSpatialPermission()
    {
        const string spatialPermission = "com.oculus.permission.USE_SCENE";
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(spatialPermission))
        {
            var callbacks = new UnityEngine.Android.PermissionCallbacks();
            callbacks.PermissionDenied += Denied;
            callbacks.PermissionGranted += Granted;

            UnityEngine.Android.Permission.RequestUserPermission(spatialPermission, callbacks);
        }
        else
        {
            Debug.Log("Permission already granted.");
        }
    }
}