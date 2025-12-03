using System.Collections.Generic;
using UnityEngine;

public class Stroke : MonoBehaviour
{
    [Header("Stroke Variants")]
    public GameObject guideStroke;
    public GameObject emptyStroke;
    public GameObject filledStroke;

    [Header("Ruta esperada del trazo")]
    [Tooltip("Lista de puntos que definen el recorrido correcto del trazo.")]
    public List<Transform> pathPoints = new List<Transform>();

    [Header("Audio al completar (opcional)")]
    public AudioClip CompletionSound;

    public void HideAll()
    {
        if (guideStroke) guideStroke.SetActive(false);
        if (emptyStroke) emptyStroke.SetActive(false);
        if (filledStroke) filledStroke.SetActive(false);
    }

    public void ShowGuideAndEmpty()
    {
        if (guideStroke) guideStroke.SetActive(true);
        if (emptyStroke) emptyStroke.SetActive(true);
        if (filledStroke) filledStroke.SetActive(false);
    }

    public void HideGuideAndEmpty()
    {
        if (guideStroke) guideStroke.SetActive(false);
        if (emptyStroke) emptyStroke.SetActive(false);
    }

    public void ShowFilled()
    {
        if (filledStroke) filledStroke.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count < 2) return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            if (pathPoints[i] && pathPoints[i + 1])
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
        }
    }
}
