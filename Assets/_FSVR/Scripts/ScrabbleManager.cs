
using DG.Tweening;
using UnityEngine;

namespace FSVR
{
    public class ScrabbleManager : MonoBehaviour
    {
        [SerializeField] private GameObject house;
        [SerializeField] private GameObject[] houseSymbols;
        [SerializeField] private GameObject[] houseTiles;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            house.SetActive(false);

        }

        public void StartGame()
        {
            DisplayHouseSymbols();
            
        }
        
        void DisplayHouseSymbols()
        {
            foreach (GameObject houseSymbol in houseSymbols)
            {
                houseSymbol.SetActive(true);
            }
            
        }

        void FinishGame()
        {
            house.SetActive(true);
        }
        
    }
}
