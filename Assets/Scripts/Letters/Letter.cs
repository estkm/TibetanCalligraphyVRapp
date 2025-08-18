using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField] private Stroke[] strokes;

    public int StrokeCount => strokes.Length;

    public Stroke GetStroke(int index)
    {
        if (index < 0 || index >= strokes.Length) return null;
        return strokes[index];
    }

    public void HideAllStrokes()
    {
        foreach (var s in strokes)
            s.HideAll();
    }
}
