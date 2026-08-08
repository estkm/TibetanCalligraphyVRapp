using UnityEngine;

namespace FSVR
{
	public class FSManager : MonoBehaviour
	{
		public static FSManager Instance { get; private set; }

		[SerializeField] private bool devMode = true;
		[SerializeField] private bool stylusMode = false;
		public bool DevMode => devMode;
		public bool StylusMode => stylusMode;

		[SerializeField] private GameObject baseGame;
		[SerializeField] private Transform xrOrigin;

		[SerializeField] private GameCanvasManager gameCanvasManager;
		[SerializeField] private BoardManager boardManager;
		[SerializeField] private ScrabbleManager scabbleManager;

		private void Awake ()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			//DontDestroyOnLoad(gameObject);

			// If the player just confirmed a table position in the AR scene, move the
			// PLAYER (not Base Game) so their real table lines up with Base Game's fixed,
			// art-directed spot in this scene. This keeps the whole room (floor, skybox,
			// decorations, UI) exactly as designed - only the player's rig shifts.
			// Skipped on a normal direct launch into this scene (TableAnchor.HasValue stays false).
			if (TableAnchor.HasValue)
			{
				RecenterPlayerOnBaseGame();
				baseGame.SetActive(true);
			}

			if (devMode)
			{
				//
				Debug.LogWarning("-- DEV Mode on!");
			}
		}

		// Moves xrOrigin so that standing where the real table was (TableAnchor) puts the
		// player exactly where Base Game already sits. Only yaw (horizontal facing) is used
		// from the saved table rotation - pitch/roll from an unlevel real table would tilt
		// the player's whole horizon in VR otherwise, which is disorienting.
		private void RecenterPlayerOnBaseGame ()
		{
			Quaternion tableYaw = Quaternion.Euler(0, TableAnchor.Rotation.eulerAngles.y, 0);
			Quaternion baseGameYaw = Quaternion.Euler(0, baseGame.transform.eulerAngles.y, 0);

			Quaternion delta = baseGameYaw * Quaternion.Inverse(tableYaw);
			Vector3 offset = baseGame.transform.position - delta * TableAnchor.Position;

			xrOrigin.SetPositionAndRotation(offset, delta);
		}

		public void ResetScene ()
		{
			print("-- ResetScene");
		}

		public void ToggleMR ()
		{
			print("-- ToggleMR");
		}
	}
}