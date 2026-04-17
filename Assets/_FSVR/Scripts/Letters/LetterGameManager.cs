using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LetterGameManager : MonoBehaviour
{
	public static LetterGameManager Instance;

	public UnityEvent OnAllLettersCompleted;

	[Header("Letters Setup")]
	public List<Letter> letters = new List<Letter>();

	[Header("Settings")]
	public float delayBetweenStrokes = 5f;

	public float delayBetweenLetters = 5f;

	[Header("Audio")]
	[SerializeField] private AudioSource audioSource;

	[SerializeField] private AudioClip WrongAudio;
	[SerializeField] private AudioClip CorrectAudio;

	[Header("Completion")]
	[SerializeField] private GameObject completionStamp;

	[Header("References")]
	public RaycastLineDrawerTrigger lineDrawer;

	private int _currentLetterIndex = -1;
	private int _currentStrokeIndex = -1;

	private bool _strokeInProgress = false;

	private void Awake ()
	{
		Instance = this;
	}

	private void Start ()
	{
		if (lineDrawer != null)
			lineDrawer.OnStrokeFinished += HandleStrokeFinished;

		if (completionStamp != null)
			completionStamp.SetActive(false);

		foreach (var letter in letters)
			letter?.HideAllStrokes();

		ShowNextLetter();
	}

	private void ShowNextLetter ()
	{
		if (_currentLetterIndex > -1)
			letters[_currentLetterIndex].gameObject.SetActive(false);

		_currentLetterIndex++;
		_currentStrokeIndex = -1;

		if (_currentLetterIndex < letters.Count)
		{
			ShowNextStroke();
		}
		else
		{
			ShowCompletionStamp();
			OnAllLettersCompleted.Invoke();
		}
	}

	private void ShowCompletionStamp ()
	{
		if (completionStamp != null)
			completionStamp.SetActive(true);

		Debug.Log("✅ Todas las letras completadas!");
	}

	private void ShowNextStroke ()
	{
		_currentStrokeIndex++;
		_strokeInProgress = true;

		Letter currentLetter = letters[_currentLetterIndex];
		currentLetter.gameObject.SetActive(true);

		if (_currentStrokeIndex < currentLetter.StrokeCount)
		{
			Stroke stroke = currentLetter.GetStroke(_currentStrokeIndex);
			stroke.ShowGuideAndEmpty();

			// **CRÍTICO — ESTA LÍNEA ES LO QUE HACÍA FALTA**
			lineDrawer.SetCurrentStroke(stroke);
		}
		else
		{
			if (_currentLetterIndex >= letters.Count - 1)
			{
				ShowCompletionStamp();
				OnAllLettersCompleted.Invoke();
			}
			else
			{
				StartCoroutine(ShowNextLetterDelayed());
			}
		}
	}

	private void HandleStrokeFinished ()
	{
		if (!_strokeInProgress) return;

		Letter currentLetter = letters[_currentLetterIndex];
		Stroke currentStroke = currentLetter.GetStroke(_currentStrokeIndex);

		OnStrokeCompleted(currentStroke);
	}

	public void OnStrokeCompleted (Stroke stroke)
	{
		if (!_strokeInProgress) return;
		_strokeInProgress = false;

		stroke.HideGuideAndEmpty();
		stroke.ShowFilled();
		audioSource.PlayOneShot(CorrectAudio);

		if (_currentStrokeIndex >= letters[_currentLetterIndex].StrokeCount - 1)
		{
			PlayLetterCompletedSound(letters[_currentLetterIndex]);
		}

		StartCoroutine(ShowNextStrokeDelayed());
	}

	private void PlayLetterCompletedSound (Letter letter)
	{
		if (letter == null) return;
		if (letter.Tile != null)
			letter.Tile.SetActive(true);

		if (audioSource != null && letter.CompletionSound != null)
			audioSource.PlayOneShot(letter.CompletionSound);
	}

	private IEnumerator ShowNextStrokeDelayed ()
	{
		yield return new WaitForSeconds(delayBetweenStrokes);
		ShowNextStroke();
	}

	private IEnumerator ShowNextLetterDelayed ()
	{
		yield return new WaitForSeconds(delayBetweenLetters);
		ShowNextLetter();
	}
}