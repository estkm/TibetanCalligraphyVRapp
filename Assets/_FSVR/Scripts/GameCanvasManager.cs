using FSVR;
using UnityEngine;
using UnityEngine.Serialization;

public class GameCanvasManager : MonoBehaviour
{
	[FormerlySerializedAs("welcomeItmes")][SerializeField] private GameObject welcomeHolder;
	[SerializeField] private GameObject baseGame;

	[Header("Scrabble")]
	[SerializeField] private GameObject scrabbleCanvas;

	[SerializeField] private ScrabbleManager scrabbleManager;
	[SerializeField] private GameObject scrabbleButton;

	private FSManager _FSManager;

	private void Awake ()
	{
		_FSManager = FSManager.Instance;

		scrabbleButton.SetActive(_FSManager.DevMode);
	}

	private void OnEnable ()
	{
		RevealWelcomeItems();
	}

	private void RevealWelcomeItems ()
	{
		welcomeHolder.SetActive(true);
		Debug.Log("--GameCanvasManager RevealWelcomeItems");
	}

	public void StartScrabble ()
	{
		baseGame.SetActive(true);
		scrabbleManager.StartGame();
	}
}