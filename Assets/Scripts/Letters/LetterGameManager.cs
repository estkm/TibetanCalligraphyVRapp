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

    [Header("Completion")]
    [SerializeField] private GameObject completionStamp; // Stamp that appears when all letters are completed

    [Header("References")]
    [Tooltip("Arrastra aquí el RaycastLineDrawerTrigger")]
    public RaycastLineDrawerTrigger lineDrawer;

    private int _currentLetterIndex = -1;
    private int _currentStrokeIndex = -1;

    // 🔒 bandera para evitar que un mismo stroke se valide más de una vez
    private bool _strokeInProgress = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (lineDrawer != null)
            lineDrawer.OnStrokeFinished += HandleStrokeFinished;

        // Hide the completion stamp at start
        if (completionStamp != null)
            completionStamp.SetActive(false);

        foreach (var letter in letters)
        {
            if (letter != null)
                letter.HideAllStrokes();
        }

        ShowNextLetter();
    }

    private void ShowNextLetter()
    {
        if (_currentLetterIndex > -1)
        {
            Letter lastLetter = letters[_currentLetterIndex];
            lastLetter.gameObject.SetActive(false);
        }

        _currentLetterIndex++;
        _currentStrokeIndex = -1;

        if (_currentLetterIndex < letters.Count)
        {
            ShowNextStroke();
        }
        else
        {
            // All letters completed! Show completion stamp and end the flow
            ShowCompletionStamp();
            OnAllLettersCompleted.Invoke();
        }
    }

    private void ShowCompletionStamp()
    {
        Debug.Log("✅ Todas las letras completadas!");
        
        // Activate the completion stamp
        if (completionStamp != null)
        {
            completionStamp.SetActive(true);
        }
        
        // The game flow ends here - no more progression
    }

    private void ShowNextStroke()
    {
        _currentStrokeIndex++;
        _strokeInProgress = true; // 🔒 ahora estamos esperando este stroke

        Letter currentLetter = letters[_currentLetterIndex];
        currentLetter.gameObject.SetActive(true);

        if (_currentStrokeIndex < currentLetter.StrokeCount)
        {
            Stroke stroke = currentLetter.GetStroke(_currentStrokeIndex);
            stroke.ShowGuideAndEmpty();
        }
        else
        {

            // Check if this was the last letter
            if (_currentLetterIndex >= letters.Count - 1)
            {
                // This was the last letter, show completion stamp
                ShowCompletionStamp();
                OnAllLettersCompleted.Invoke();
            }
            else
            {
                // Continue to next letter
                StartCoroutine(ShowNextLetterDelayed());
            }
        }
    }

    private void HandleStrokeFinished()
    {
        // ✅ ignorar si no hay stroke activo
        if (!_strokeInProgress) return;

        Letter currentLetter = letters[_currentLetterIndex];
        Stroke currentStroke = currentLetter.GetStroke(_currentStrokeIndex);

        OnStrokeCompleted(currentStroke);
    }

    public void OnStrokeCompleted(Stroke stroke)
    {
        if (!_strokeInProgress) return; // seguridad extra

        _strokeInProgress = false; // ✅ este stroke ya está validado

        stroke.HideGuideAndEmpty();
        stroke.ShowFilled();

        // Check if this was the last stroke of the letter
        if (_currentStrokeIndex >= letters[_currentLetterIndex].StrokeCount - 1)
        {
            // Letter completed! Play the specific completion sound for this letter immediately
            PlayLetterCompletedSound(letters[_currentLetterIndex]);
        }

        StartCoroutine(ShowNextStrokeDelayed());
    }

    private void PlayLetterCompletedSound(Letter letter)
    {
        if (audioSource != null && letter != null && letter.CompletionSound != null)
        {
            audioSource.PlayOneShot(letter.CompletionSound);
        }
    }

    private IEnumerator ShowNextStrokeDelayed()
    {
        yield return new WaitForSeconds(delayBetweenStrokes);
        ShowNextStroke();
    }

    private IEnumerator ShowNextLetterDelayed()
    {
        yield return new WaitForSeconds(delayBetweenLetters);
        ShowNextLetter();
    }
}
