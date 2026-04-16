using FSVR;
using UnityEngine;

public class FSManager : MonoBehaviour
{
	public static FSManager Instance { get; private set; }

	[Header("Settings")]
	[SerializeField] private bool devMode = true;

	public bool DevMode => devMode;

	[Header("References")]
	[SerializeField] private GameObject baseGame;

	[SerializeField] private GameCanvasManager gameCanvasManager;
	[SerializeField] private BoardManager boardManager;
	[SerializeField] private ScrabbleManager scabbleManager;

	[Header("MX Ink")]
	[SerializeField] private MXInkStylusHandler MXInk;

	private void Awake ()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		if (devMode)
		{
			//
			Debug.LogWarning("-- DEV Mode on!");
		}

		baseGame.SetActive(false);
	}

	public void TestLoad ()
	{
		print("-- TestLoad");
	}
}