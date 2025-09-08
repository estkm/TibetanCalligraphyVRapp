using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnClick;
    public UnityEvent OnHover;
    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }

    private void OnMouseOver()
    {
        OnHover?.Invoke();
    }
}
