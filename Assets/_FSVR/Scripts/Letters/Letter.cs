using UnityEngine;

public class Letter : MonoBehaviour
{
    [Header("Letter Audio")]
    [SerializeField] private AudioClip completionSound; // Audio that plays when this letter is completed
    
    [Header("Letter Components")]
    [SerializeField] private Stroke[] strokes;

    [Header("Letter Tile")]
    [SerializeField] private GameObject tile;

    public int StrokeCount => strokes.Length;
    
    public AudioClip CompletionSound => completionSound;

    public GameObject Tile => tile;

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
