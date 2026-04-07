using System;
using DG.Tweening;
using FSVR;
using UnityEngine;
using UnityEngine.Serialization;

public class GameCanvasManager : MonoBehaviour
{
    [FormerlySerializedAs("welcomeItmes")] [SerializeField] private GameObject welcomeHolder;
    [SerializeField] private GameObject baseGame;
    
    [Header("Scrabble")]
    [SerializeField] private GameObject scrabbleCanvas;
    [SerializeField] private ScrabbleManager scrabbleManager;
    
    private void OnEnable()
    {
        RevealWelcomeItems();
        
    }

    public void RevealWelcomeItems()
    {
        welcomeHolder.SetActive(true);
        Debug.Log("--GameCanvasManager RevealWelcomeItems");
        //DOTween.To(() => welcomeItmes.transform.localScale, x => welcomeItmes.transform.localScale = x, new Vector3(1, 1, 1), 0.5f);
        
    }

    public void StartScrabble()
    {
        //welcomeItmes.SetActive(false);
        baseGame.SetActive(true);
        scrabbleManager.StartGame();
        
    }
    
    
}
