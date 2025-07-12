using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RaycastLineDrawer : MonoBehaviour
{
    [SerializeField] private Transform drawingPointer;
    [SerializeField] private Transform boardContainer;
    [SerializeField] private float rayDistance = 0.05f;
    [SerializeField] private LayerMask drawLayerMask; // Asigna la layer de la pizarra
    [SerializeField] private Transform lineRedererPrefab;

    private Vector3 _drawPoint;
    private bool _isDrawing = false;
    private bool _recognized = false;

    private int _strokeId = -1;
    private int _vertexCount = 0;

    private List<LineRenderer> _linesRenderer = new List<LineRenderer>();
    private LineRenderer _currentLineRenderer;

    private void Update()
    {
        DrawLine();
    }

    private void DrawLine()
    {
        Ray ray = new Ray(drawingPointer.position, drawingPointer.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, drawLayerMask))
        {
            _drawPoint = hit.point;

            if (!_isDrawing)
            {
                _isDrawing = true;

                if (_recognized)
                {
                    _recognized = false;
                    _strokeId = -1;

                    foreach (var lineRenderer in _linesRenderer)
                    {
                        Destroy(lineRenderer.gameObject);
                    }
                    _linesRenderer.Clear();
                }

                ++_strokeId;

                Transform tempGO = Instantiate(lineRedererPrefab, Vector3.zero, Quaternion.identity);
                tempGO.SetParent(boardContainer);
                _currentLineRenderer = tempGO.GetComponent<LineRenderer>();
                _linesRenderer.Add(_currentLineRenderer);
                _vertexCount = 0;
            }

            _currentLineRenderer.positionCount = ++_vertexCount;
            _currentLineRenderer.SetPosition(_vertexCount - 1, _drawPoint);
        }
        else
        {
            _isDrawing = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (drawingPointer != null)
        {
            Gizmos.color = Color.green;
            Vector3 origin = drawingPointer.position;
            Vector3 direction = drawingPointer.forward * rayDistance;

            Gizmos.DrawLine(origin, origin + direction);
        }
    }
}
