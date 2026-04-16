using FSVR;
using UnityEngine;
using UnityEngine.SceneManagement;

// Attach to any GameObject inside the Canvas.
// Wire the Button OnClick -> this component -> ReloadScene()
public class ResetSceneButton : MonoBehaviour
{
	public void ReloadScene ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		FSManager.Instance.ResetScene();
	}
}