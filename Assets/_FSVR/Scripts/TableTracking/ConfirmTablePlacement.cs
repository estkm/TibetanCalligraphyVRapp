using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

// Hooked to the AR scene's Play button. Saves exactly where the player left
// the placeholder table (after grabbing/adjusting it) and hands that
// position off to the VR scene, where Base Game gets placed there.
public class ConfirmTablePlacement : MonoBehaviour
{
    [SerializeField] private PlanePrefabSpawner spawner;
    [SerializeField] private string vrSceneName = "ForeverSnowland Stylus";

    // Wired to the Play button's OnClick.
    public void OnPlayPressed()
    {
        Transform placedTable = spawner.SpawnedTransform;

        // Guards against pressing Play before the table has even been detected/spawned.
        if (placedTable == null)
        {
            Debug.LogError("ConfirmTablePlacement: no table has been placed yet, ignoring Play.");
            return;
        }

        TableAnchor.Save(placedTable);

        // Hide it right away - position/rotation are already captured above, so
        // nothing is lost. Avoids it visibly sticking to the camera during the
        // moment between pressing Play and the scene actually unloading.
        placedTable.gameObject.SetActive(false);

        ShutDownARSession();

        SceneManager.LoadScene(vrSceneName);
    }

    // Tears AR down here rather than letting the scene unload do it. ARInputManager
    // (on the AR Session object) stops the XRInputSubsystem in OnDisable, so this
    // brings that stop forward to a point where it can be undone in the same frame -
    // the VR scene then opens with live tracking instead of a frozen camera.
    private void ShutDownARSession()
    {
        ARSession session = FindAnyObjectByType<ARSession>();

        if (session != null)
            session.gameObject.SetActive(false);

        XRInputSubsystemGuard.RestoreNow();
    }
}
