using UnityEngine;

namespace FSVR
{
	public class FSManager : MonoBehaviour
	{
		public static FSManager Instance { get; private set; }

		[SerializeField] private bool devMode = true;
		public bool DevMode => devMode;

		[SerializeField] private GameObject baseGame;

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

			if (devMode)
			{
				//
				Debug.LogWarning("-- DEV Mode on!");
			}
		}

		public void ResetScene ()
		{
			print("-- ResetScene");
		}
	}
}