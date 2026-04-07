
using System.Text;
using DG.Tweening;
using UnityEngine;

namespace FSVR
{
    public class ScrabbleManager : MonoBehaviour
    {
        [SerializeField] private GameObject house;
        [SerializeField] private GameObject[] houseSymbols;
        [SerializeField] private GameObject[] houseTiles;
        
        [SerializeField] private TileSlot[] slots;
        [SerializeField] private string targetWord = "CAT";
        
        private FSManager _fsManager;
        
        //game status enum
        private enum GameStatus { Waiting, Playing, Done}
        private GameStatus gameStatus = GameStatus.Waiting;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _fsManager = FSManager.Instance;
            //_fsManager.TestLoad();
            
            house.SetActive(false);
        }

        public void StartGame()
        {
            gameStatus = GameStatus.Playing;
            gameObject.SetActive(true);
            DisplayHouseSymbols();
            
        }
        
        public string GetCurrentWord()
        {
            var sb = new StringBuilder();

            foreach (var slot in slots)
            {
                if (!slot.currentSymbolTile)
                {
                    sb.Append("_");
                }
                else
                {
                    sb.Append(slot.currentSymbolTile.Symbol);
                }
            }

            return sb.ToString();
        }
        
        public bool IsCorrectWord()
        {
            if (!AreAllSlotsFilled())
                return false;

            var builtWord = "";

            foreach (TileSlot slot in slots)
            {
                builtWord += slot.currentSymbolTile.Symbol;
            }

            return string.Equals(builtWord, targetWord, System.StringComparison.OrdinalIgnoreCase);
        }
        
        public bool AreAllSlotsFilled()
        {
            foreach (TileSlot slot in slots)
            {
                if (slot.currentSymbolTile == null)
                    return false;
            }

            return true;
        }
        
        public void CheckWord()
        {
            var current = GetCurrentWord();
            Debug.Log("Current sequence: " + current);

            if (IsCorrectWord())
            {
                Debug.Log("Correct word!");
            }
            else
            {
                Debug.Log("Not correct.");
            }
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
