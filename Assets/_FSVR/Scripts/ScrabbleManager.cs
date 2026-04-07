
using DG.Tweening;
using UnityEngine;

namespace FSVR
{
    public class ScrabbleManager : MonoBehaviour
    {
        [SerializeField] private GameObject house;
        [SerializeField] private GameObject[] houseSymbols;
        [SerializeField] private GameObject[] houseTiles;
        
        private FSManager _fsManager;

        //private bool gameEnd = false;
        //private bool gameStarted = false;
        
        //game status enum
        private enum GameStatus { Waiting, Playing, Done}
        private GameStatus gameStatus = GameStatus.Waiting;
        
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _fsManager = FSManager.Instance;
            _fsManager.TestLoad();
            house.SetActive(false);
        }

        public void StartGame()
        {
            gameStatus = GameStatus.Playing;
            gameObject.SetActive(true);
            DisplayHouseSymbols();
            
        }

        void CompareSequence()
        {
            
        }
        
        void DisplayHouseSymbols()
        {
            foreach (var houseSymbol in houseSymbols)
            {
                houseSymbol.SetActive(true);
            }
        }

        void DisplayHouse()
        {
            house.SetActive(true);
        }

        void FinishGame()
        {
            gameStatus = GameStatus.Done;
            DisplayHouse();
        }
        
    }
}
