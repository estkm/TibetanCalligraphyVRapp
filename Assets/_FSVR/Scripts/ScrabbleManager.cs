using UnityEngine;

namespace FSVR
{
    public class ScrabbleManager : MonoBehaviour
    {
        [SerializeField] private GameObject house;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            house.SetActive(false);

        }

        
    }
}
