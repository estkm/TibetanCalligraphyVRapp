using System;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLineDrawerTrigger : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform drawingPointer;
    [SerializeField] private float rayDistance = 0.05f;
    [SerializeField] private LayerMask drawLayerMask;

    [Header("Line Renderer")]
    [SerializeField] private Transform boardContainer;
    [SerializeField] private Transform lineRendererPrefab;

    [Header("Validation Settings")]
    [Tooltip("Distancia máxima para considerar que se tocó un punto correcto del trazo.")]
    [SerializeField] private float pointThreshold = 0.025f;

    private Vector3 _drawPoint;
    private bool _isDrawing = false;
    private LineRenderer _currentLineRenderer;
    private int _vertexCount = 0;

    // Stroke actual
    private Stroke _currentStroke;
    private int _currentPathIndex = 0;

    // Evento para el GameManager
    public Action OnStrokeFinished;

    private void Update() => DrawLogic();

    // ======================================================
    //  API para que el GameManager indique qué stroke validar
    // ======================================================
    public void SetCurrentStroke(Stroke stroke)
    {
        _currentStroke = stroke;
        _currentPathIndex = 0;
    }

    // ======================================================
    //  LÓGICA DE DIBUJO + VALIDACIÓN
    // ======================================================
    private void DrawLogic()
    {
        Ray ray = new Ray(drawingPointer.position, drawingPointer.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, drawLayerMask))
        {
            _drawPoint = hit.point;

            // Inicio del trazo
            if (!_isDrawing)
            {
                _isDrawing = true;

                Transform tempLine = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);
                tempLine.SetParent(boardContainer);
                boardContainer.rotation = Quaternion.Euler(90, 0, 0);
                tempLine.SetParent(boardContainer.parent);
                boardContainer.rotation = Quaternion.identity;

                _currentLineRenderer = tempLine.GetComponent<LineRenderer>();
                _vertexCount = 0;
            }

            // Añadir punto a la línea
            _currentLineRenderer.positionCount = ++_vertexCount;
            Vector3 p = _drawPoint;
            p.z -= 0.002f;
            _currentLineRenderer.SetPosition(_vertexCount - 1, p);

            // Validar checkpoints
            ValidateStrokePoint(_drawPoint);
        }
        else
        {
            if (_isDrawing)
            {
                FinishStroke();
            }
        }
    }

    // ======================================================
    //  VALIDACIÓN REAL
    // ======================================================
    private void ValidateStrokePoint(Vector3 drawPosition)
    {
        if (_currentStroke == null) return;
        if (_currentStroke.pathPoints.Count == 0) return;

        if (_currentPathIndex >= _currentStroke.pathPoints.Count)
            return;

        Vector3 targetPoint = _currentStroke.pathPoints[_currentPathIndex].position;

        float distance = Vector3.Distance(drawPosition, targetPoint);

        if (distance <= pointThreshold)
        {
            _currentPathIndex++;
        }
    }

    // ======================================================
    //  FIN DEL TRAZO
    // ======================================================
    private void FinishStroke()
    {
        _isDrawing = false;

        if (_currentLineRenderer != null)
            Destroy(_currentLineRenderer.gameObject);

        bool strokeValid = false;

        if (_currentStroke != null && _currentPathIndex >= _currentStroke.pathPoints.Count)
        {
            strokeValid = true;
        }

        if (strokeValid)
            OnStrokeFinished?.Invoke();

        _currentPathIndex = 0;
    }

    private void OnDrawGizmos()
    {
        if (drawingPointer == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(drawingPointer.position, drawingPointer.position + drawingPointer.forward * rayDistance);
    }
}
