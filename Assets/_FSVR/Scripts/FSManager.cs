using UnityEngine;

public class FSManager : MonoBehaviour
{
    public static FSManager Instance { get; private set; }

    [SerializeField] private GameObject baseGame;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TestLoad()
    {
        print("-- TestLoad");
    }
}
