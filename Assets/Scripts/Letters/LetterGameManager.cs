using System;
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
    public float delayBetweenLetters = 2f;

    private int _currentLetterIndex = -1;

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
                letter.HideAll();
        }

        ShowNextLetter(); // Start with the first letter
    }

    private void ShowNextLetter()
    {
        _currentLetterIndex++;

        if (_currentLetterIndex < letters.Count)
        {
            Letter currentLetter = letters[_currentLetterIndex];
            currentLetter.StartDrawing(); // let the letter handle its strokes
        }
        else
        {
            Debug.Log("✅ All letters completed!");
        }
    }

    public void OnLetterCompleted(Letter letter)
    {
        // Wait before showing the next letter
        StartCoroutine(ShowNextLetterDelayed());
    }

    private IEnumerator ShowNextLetterDelayed()
    {
        yield return new WaitForSeconds(delayBetweenLetters);
        ShowNextLetter();
    }
}
