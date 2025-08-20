using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGameManager : MonoBehaviour
{
    public static LetterGameManager Instance;

    [Header("Letters Setup")]
    public List<Letter> letters = new List<Letter>();

    [Header("Settings")]
    public float delayBetweenStrokes = 5f;
    public float delayBetweenLetters = 5f;

    [Header("References")]
    [Tooltip("Arrastra aquí el RaycastLineDrawerTrigger")]
    public RaycastLineDrawerTrigger lineDrawer;

    private int _currentLetterIndex = -1;
    private int _currentStrokeIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Suscribirse al evento del drawer
        if (lineDrawer != null)
            lineDrawer.OnStrokeFinished += HandleStrokeFinished;

        // Ocultar todo al inicio
        foreach (var letter in letters)
        {
            if (letter != null)
                letter.HideAllStrokes();
        }

        ShowNextLetter(); // Arrancamos con la primera letra
    }

    private void ShowNextLetter()
    {
        if (_currentLetterIndex > -1){
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
            Debug.Log("✅ Todas las letras completadas!");
        }
    }

    private void ShowNextStroke()
    {

        _currentStrokeIndex++;
        

        Letter currentLetter = letters[_currentLetterIndex];
        currentLetter.gameObject.SetActive(true);
        if (_currentStrokeIndex < currentLetter.StrokeCount)
        {
            Stroke stroke = currentLetter.GetStroke(_currentStrokeIndex);
            stroke.ShowGuideAndEmpty();
        }
        else
        {
            StartCoroutine(ShowNextLetterDelayed());
        }
    }

    private void HandleStrokeFinished()
    {
        Letter currentLetter = letters[_currentLetterIndex];
        Stroke currentStroke = currentLetter.GetStroke(_currentStrokeIndex);

        OnStrokeCompleted(currentStroke);
    }

    // 🔥 llamado cuando un stroke se completa
    public void OnStrokeCompleted(Stroke stroke)
    {
        stroke.HideGuideAndEmpty();
        stroke.ShowFilled();

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
