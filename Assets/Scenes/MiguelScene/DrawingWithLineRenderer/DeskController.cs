using UnityEngine;

public class DeskController : MonoBehaviour
{
    [SerializeField] private Transform drawingPointer;
    [SerializeField] private float rayDistance = 0.05f;
    [SerializeField] private Transform deskSurface;

    public void ChangeYPosition()
    {
        Ray ray = new Ray(drawingPointer.position, drawingPointer.forward);
        
        Vector3 pointInSpace = ray.origin + ray.direction * rayDistance;
        
        Vector3 newPos = deskSurface.position;
        newPos = pointInSpace;
        deskSurface.position = newPos;
    }
}
