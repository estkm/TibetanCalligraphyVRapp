using System;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLineDrawer : MonoBehaviour
{
    [SerializeField] private Transform drawingPointer;
    [SerializeField] private float rayDistance = 0.05f;
    [SerializeField] private LayerMask drawLayerMask;
    [SerializeField] private Transform lineRendererPrefab;

    private Vector3 _drawPoint;
    private bool _isDrawing = false;

    private int _strokeId = -1;
    private int _vertexCount = 0;

    private List<LineRenderer> _linesRenderer = new List<LineRenderer>();
    private LineRenderer _currentLineRenderer;

    // 🔔 The stroke currently being traced (set by LetterGameManager)
    public Stroke ActiveStroke { get; set; }

    // 🔔 Event triggered when a stroke is finished
    public event Action<int, LineRenderer, Stroke> OnStrokeFinished;

    private void Update()
    {
        DrawLine();
    }

    private void DrawLine()
    {
        if (ActiveStroke == null) return; // no active stroke

        Ray ray = new Ray(drawingPointer.position, drawingPointer.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, drawLayerMask))
        {
            _drawPoint = hit.point;

            if (!_isDrawing)
            {
                _isDrawing = true;
                ++_strokeId;

                Transform tempGO = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);
                tempGO.SetParent(ActiveStroke.transform, false); // parent to the stroke
                _currentLineRenderer = tempGO.GetComponent<LineRenderer>();
                _linesRenderer.Add(_currentLineRenderer);
                _vertexCount = 0;
            }

            _currentLineRenderer.positionCount = ++_vertexCount;
            _drawPoint.z -= 0.002f; // Slight offset to avoid z-fighting
            _currentLineRenderer.SetPosition(_vertexCount - 1, _drawPoint);
        }
        else
        {
            if (_isDrawing)
            {
                _isDrawing = false;
                // Notify subscribers that the stroke finished
                OnStrokeFinished?.Invoke(_strokeId, _currentLineRenderer, ActiveStroke);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawingPointer != null)
        {
            Gizmos.color = Color.green;
            Vector3 origin = drawingPointer.position;
            Vector3 direction = drawingPointer.forward * rayDistance;
            Gizmos.DrawLine(origin, origin + direction); // debug ray
        }
    }
}
