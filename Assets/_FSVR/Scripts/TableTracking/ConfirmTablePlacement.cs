using UnityEngine;
using UnityEngine.SceneManagement;

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

        SceneManager.LoadScene(vrSceneName);
    }
}
