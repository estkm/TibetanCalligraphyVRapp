using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGameManager : MonoBehaviour
{
    public static LetterGameManager Instance;

    [Header("Letters Setup (set manually in Inspector)")]
    [Tooltip("Add each Letter manually in the correct order")]
    public List<Letter> letters = new List<Letter>();

    [Header("Settings")]
    public float delayBetweenStrokes = 5f;
    public float delayBetweenLetters = 5f;

    private int _currentLetterIndex = -1;
    private int _currentStrokeIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Hide all letters at the start
        foreach (var letter in letters)
        {
            if (letter != null)
                letter.HideAllStrokes();
        }

        ShowNextLetter(); // Start with the first letter
    }

    private void ShowNextLetter()
    {
        _currentLetterIndex++;
        _currentStrokeIndex = -1;

        if (_currentLetterIndex < letters.Count)
        {
            ShowNextStroke(); // Start with first stroke of this letter
        }
        else
        {
            Debug.Log("✅ All letters completed!");
        }
    }

    private void ShowNextStroke()
    {
        _currentStrokeIndex++;

        Letter currentLetter = letters[_currentLetterIndex];

        if (_currentStrokeIndex < currentLetter.StrokeCount)
        {
            Stroke stroke = currentLetter.GetStroke(_currentStrokeIndex);
            stroke.ShowGuideAndEmpty(); // Show guide + empty together
        }
        else
        {
            // Finished all strokes in this letter
            StartCoroutine(ShowNextLetterDelayed());
        }
    }

    // Called by RaycastLineDrawer when a stroke is completed
    public void OnStrokeCompleted(Stroke stroke)
    {
        // Hide Guide + Empty and show Filled
        stroke.HideGuideAndEmpty();
        stroke.ShowFilled();

        // Wait before showing the next stroke
        StartCoroutine(ShowNextStrokeDelayed());
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
