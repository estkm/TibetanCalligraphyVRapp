using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGameManager : MonoBehaviour
{
    [SerializeField] private List<Letter> letters;
    [SerializeField] private float guideTime = 5f;
    [SerializeField] private float nextLetterDelay = 5f;

    private int _currentLetterIndex = 0;
    private int _currentStrokeIndex = 0;

    private RaycastLineDrawer drawer;

    private void Awake()
    {
        drawer = FindObjectOfType<RaycastLineDrawer>();
    }

    private void Start()
    {
        StartCoroutine(RunFlow());
    }

    private IEnumerator RunFlow()
    {
        while (_currentLetterIndex < letters.Count)
        {
            Letter currentLetter = letters[_currentLetterIndex];
            currentLetter.gameObject.SetActive(true);
            currentLetter.HideAllStrokes();

            for (_currentStrokeIndex = 0; _currentStrokeIndex < currentLetter.StrokeCount; _currentStrokeIndex++)
            {
                Stroke stroke = currentLetter.GetStroke(_currentStrokeIndex);
                drawer.ActiveStroke = stroke;

                // 1. Show guide
                stroke.ShowGuide();
                yield return new WaitForSeconds(guideTime);

                // 2. Show empty
                stroke.ShowEmpty();

                // Wait until the player finishes the stroke
                bool finished = false;

                void OnStrokeEnd(int id, LineRenderer line, Stroke s)
                {
                    if (s == stroke) // only accept the current stroke
                        finished = true;
                }

                drawer.OnStrokeFinished += OnStrokeEnd;

                yield return new WaitUntil(() => finished);

                drawer.OnStrokeFinished -= OnStrokeEnd;

                // 3. Show filled
                stroke.ShowFilled();

                yield return new WaitForSeconds(guideTime);
            }

            // After finishing all strokes, hide letter and wait before next
            currentLetter.gameObject.SetActive(false);
            yield return new WaitForSeconds(nextLetterDelay);

            _currentLetterIndex++;
        }
    }
}
