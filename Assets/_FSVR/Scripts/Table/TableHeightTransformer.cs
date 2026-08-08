using UnityEngine;

public class TableHeightControl : MonoBehaviour
{
    [Header("Movement Limits")]
    [Tooltip("Left Movement")]
    [SerializeField] private float minXmovementL = -0.1f;
    [Tooltip("Right Movement")]
    [SerializeField] private float maxXmovementL = 0.1f;
    [Space(10)]
    [Tooltip("Back Movement")]
    [SerializeField] private float minYmovementB = 0f;
    [Tooltip("Front Movement")]
    [SerializeField] private float maxYmovementF = 0.5f;
    [Space(10)]
    [Tooltip("Down Movement")]
    [SerializeField] private float minHeightD = 0.5f;
    [Tooltip("Up Movement")]
    [SerializeField] private float maxHeightUp = 1.5f;
    
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }


    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // Lock X and Z
        //pos.x = startPosition.x;
        //pos.z = startPosition.z;
        
        // Limit movement
        pos.x = Mathf.Clamp(pos.x, startPosition.x + minXmovementL, startPosition.x + maxXmovementL);
        pos.z = Mathf.Clamp(pos.z, startPosition.z + minYmovementB, startPosition.z + maxYmovementF);
        //Limit height
        pos.y = Mathf.Clamp(pos.y, minHeightD, maxHeightUp);

        transform.position = pos;
    }
}