using UnityEngine;

public class Stroke : MonoBehaviour
{
    [SerializeField] private GameObject emptyObj;
    [SerializeField] private GameObject filledObj;
    [SerializeField] private GameObject guideObj;

    public void ShowGuide()
    {
        guideObj.SetActive(true);
        emptyObj.SetActive(false);
        filledObj.SetActive(false);
    }

    public void ShowEmpty()
    {
        guideObj.SetActive(false);
        emptyObj.SetActive(true);
        filledObj.SetActive(false);
    }

    public void ShowFilled()
    {
        guideObj.SetActive(false);
        emptyObj.SetActive(false);
        filledObj.SetActive(true);
    }

    public void HideAll()
    {
        guideObj.SetActive(false);
        emptyObj.SetActive(false);
        filledObj.SetActive(false);
    }
}
