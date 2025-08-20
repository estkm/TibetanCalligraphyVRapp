using UnityEngine;

public class Stroke : MonoBehaviour
{
    [Header("Stroke Variants")]
    public GameObject guideStroke;
    public GameObject emptyStroke;
    public GameObject filledStroke;

    public void HideAll()
    {
        if (guideStroke != null) guideStroke.SetActive(false);
        if (emptyStroke != null) emptyStroke.SetActive(false);
        if (filledStroke != null) filledStroke.SetActive(false);
    }

    public void ShowGuideAndEmpty()
    {
        if (guideStroke != null) guideStroke.SetActive(true);
        if (emptyStroke != null) emptyStroke.SetActive(true);
        if (filledStroke != null) filledStroke.SetActive(false);
    }

    public void HideGuideAndEmpty()
    {
        if (guideStroke != null) guideStroke.SetActive(false);
        if (emptyStroke != null) emptyStroke.SetActive(false);
    }

    public void ShowFilled()
    {
        if (filledStroke != null) filledStroke.SetActive(true);
    }
}
