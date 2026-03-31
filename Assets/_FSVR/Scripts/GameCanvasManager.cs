using System;
using DG.Tweening;
using FSVR;
using UnityEngine;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject welcomeItmes;
    [SerializeField] private ScrabbleManager scrabbleManager;
    [SerializeField] private GameObject baseGame;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        RevealWelcomeItems();
        //Debug.Log("--GameCanvasManager OnEnable");
        
        
    }

    public void RevealWelcomeItems()
    {
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
