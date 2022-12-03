using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _boundsExpand;

    [Inject] private MapGenerator _mapGenerator;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        _mapGenerator.OnGeneratedEvent += FitToMap;
    }
    private void OnDisable()
    {
        _mapGenerator.OnGeneratedEvent -= FitToMap;

    }

    public void FitToMap()
    {
        Bounds bounds = _mapGenerator.Map.Bounds;

        bounds.Expand(_boundsExpand);

        float horizontal = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth;
        float vertical = bounds.size.z;

        float size = Mathf.Max(horizontal, vertical) / 2;

        _camera.orthographicSize = size;
    }
}
