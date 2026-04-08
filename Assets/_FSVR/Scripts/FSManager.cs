using UnityEngine;

public class FSManager : MonoBehaviour
{
    public static FSManager Instance { get; private set; }
    
    [SerializeField] private bool devMode = true;
    public bool DevMode => devMode;

    [SerializeField] private GameObject baseGame;
    [SerializeField] private GameCanvasManager gameCanvasManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (devMode)
        {
            //
            Debug.LogWarning("-- DEV Mode on!");
            
        }
    }

    public void TestLoad()
    {
        print("-- TestLoad");
    }
    
    
}
