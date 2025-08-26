using System;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [Header("Raycast Drawing")]
    [SerializeField] private Transform drawingPointer;
    [SerializeField] private Transform boardContainer;
    [SerializeField] private float rayDistance = 0.05f;
    [SerializeField] private LayerMask drawLayerMask;
    [SerializeField] private Transform lineRendererPrefab;
    [SerializeField] private string newGestureName = "";
    [SerializeField] private List<StrokesReference> strokes;

    private Vector3 _drawPoint;
    private bool _isDrawing = false;

    private int _strokeId = -1;
    private int _vertexCount = 0;

    private List<LineRenderer> _linesRenderer = new List<LineRenderer>();
    private LineRenderer _currentLineRenderer;
    
    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Point> points = new List<Point>();
    private string message;
    
    private Material transparentMaterial;
    private int _lineRenderedRecognizedCount = 0;

    private void Start()
    {
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
        
        transparentMaterial = new Material(Shader.Find("Sprites/Default"));
        transparentMaterial.color = new Color(1f, 1f, 1f, 0f);
    }

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
                ++_strokeId;

                Transform tempGO = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity, boardContainer);
                tempGO.localRotation = Quaternion.identity;
                _currentLineRenderer = tempGO.GetComponent<LineRenderer>();
                _linesRenderer.Add(_currentLineRenderer);

                _vertexCount = 0;
            }

            _currentLineRenderer.positionCount = ++_vertexCount;

            _drawPoint += hit.normal * 0.002f;

            _currentLineRenderer.SetPosition(_vertexCount - 1, _drawPoint);

            points.Add(new Point(_drawPoint.x, _drawPoint.z, _strokeId));
        }
        else if (_isDrawing)
        {
            _isDrawing = false;
            //Recognize();
        }
    }

    public void Recognize()
    {
        if (points.Count > 0)
        {
            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

            foreach (var stroke in strokes)
            {
                if (stroke.StokeName == gestureResult.GestureClass && gestureResult.Score >= 0.94f)
                {
                    stroke.Reference.SetActive(true);

                    _lineRenderedRecognizedCount++;
                    LineRenderer lastLine = _linesRenderer[_linesRenderer.Count - 1];
   
                    lastLine.material = transparentMaterial;
                    
                    break;
                }
            }
            
            message = gestureResult.GestureClass + " " + gestureResult.Score;

            Debug.Log("Reconocido: " + message);
        }
    }

    public void Clean()
    {
        if (_lineRenderedRecognizedCount <= 0)
        {
            foreach (LineRenderer lineRenderer in _linesRenderer)
                Destroy(lineRenderer.gameObject);
            
            _linesRenderer.Clear();
            _strokeId = -1;
            return;
        }
        
        for (int i = _linesRenderer.Count - 1; i >= _lineRenderedRecognizedCount; i--)
        {
            Destroy(_linesRenderer[i].gameObject);
            _linesRenderer.RemoveAt(i);
        }
        
        _strokeId = _linesRenderer.Count - 1;
        points.RemoveAll(p => p.StrokeID >= _lineRenderedRecognizedCount);
    }

    public void AddGesture()
    {
        string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime());
        
        GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);

        trainingSet.Add(new Gesture(points.ToArray(), newGestureName));

        newGestureName = "";
    }
    
    public void RemoveLastLine()
    {
        if (_linesRenderer.Count > 0)
        {
            int lastIndex = _linesRenderer.Count - 1;
            
            if (lastIndex < _lineRenderedRecognizedCount)
                return;
            
            LineRenderer lastLine = _linesRenderer[lastIndex];

            _linesRenderer.RemoveAt(lastIndex);
            
            Destroy(lastLine.gameObject);

            points.RemoveAll(p => p.StrokeID == lastIndex);
            
            _strokeId = _linesRenderer.Count - 1;
        }
    }
}

[Serializable]
public class StrokesReference
{
    public string StokeName;
    public GameObject Reference;
}
