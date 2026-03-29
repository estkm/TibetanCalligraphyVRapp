
using UnityEngine;

namespace FSVR
{
    public class ScrabbleManager : MonoBehaviour
    {
        [SerializeField] private GameObject house;
        [SerializeField] private GameObject[] houseSymbols;

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
            
        }

        void FinishGame()
        {
            house.SetActive(true);
        }
        
    }
}
