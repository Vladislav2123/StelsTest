using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Zenject;
using System.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public event Action OnGeneratedEvent;

    [Header("Prefabs")]
    [SerializeField] private GameObject _groundBlockPrefab;
    [SerializeField] private GameObject _wallBlockPrefab;
    [SerializeField] private GameObject _startBlockPrefab;
    [SerializeField] private GameObject _finishBlockPrefab;
    [Space]
    [SerializeField] private Player _playerPrefab;
    [Header("Generation")]
    [SerializeField] private Vector2Int _size;
    [SerializeField, Range(0, 1)] private float _wallSpawnNoiseValue;
    [SerializeField] private bool _generateOnStart;
    [Header("Noise")]
    [SerializeField] private bool _displayNoise;
    [SerializeField] private Vector2Int _noiseSize;
    [SerializeField, Range(0,1)] private float _noiseScalePercent;
    [SerializeField] private Vector2 _maxNoiseOffset = new Vector2(99999, 99999);

    [Inject] private DiContainer _diContainer;
    [Inject] private NavMeshSurface _navMeshSurface;
    [Inject] private MapNoiseDisplay _noiseDisplay;

    private Vector2 _noiseOffset;
    private Vector3 _startPoint;
    private Vector3 _finishPoint;

    private const float BLOCK_RADIUS = 0.5f;

    public Map Map { get; private set; }
    public bool IsMapGenerated => Map != null;


    private void Start()
    {
        if (_generateOnStart) GenerateMap();
    }

    public async void GenerateMap()
    {
        ResetMap();
        GenerateNoiseTexture();
        GenerateModel();

        await Task.Yield();

        BakeNavMesh();

        if (CheckPathAvailability(_startPoint, _finishPoint) == false)
        {
            GenerateMap();
            return;
        }

        OnGeneratedEvent?.Invoke();
    }

    private void ResetMap()
    {
        if (Map == null) return;

        Destroy(Map.Object);
        Map = null;
    }

    private void GenerateNoiseTexture()
    {
        _noiseOffset = new Vector2(Random.Range(0, _maxNoiseOffset.x), Random.Range(0, _maxNoiseOffset.y));

        if(_displayNoise == false)
        {
            _noiseDisplay.TryHide();
            return;
        }

        _noiseDisplay.TryShow();

        Texture2D noiseTexture = new Texture2D(_noiseSize.x, _noiseSize.y);

        for (int x = 0; x < _noiseSize.x; x++)
        {
            for (int y = 0; y < _noiseSize.y; y++)
            {
                noiseTexture.SetPixel(x, y, SampleNoiseColor(x, y));
            }
        }

        noiseTexture.Apply();
        _noiseDisplay.DisplayNoise(noiseTexture);
    }

    private void GenerateModel()
    {
        Vector3 spawnPoint;
        Vector2Int borderedSize = new Vector2Int(_size.x + 2, _size.y + 2);
        Vector3 offset = new Vector2(-(float)borderedSize.x / 2 + BLOCK_RADIUS, -(float)borderedSize.y / 2 + BLOCK_RADIUS);

        Map = new Map(borderedSize, transform);

        for (int x = 0; x < borderedSize.x; x++)
        {
            for (int y = 0; y < borderedSize.y; y++)
            {
                spawnPoint = new Vector3(x + offset.x, 0, y + offset.y);

                GameObject ground = _groundBlockPrefab;

                bool isStartPoint = x == 1 && y == 1;
                bool isFinishPoint = x == borderedSize.x - 1 && y == borderedSize.y - 2;

                if (isStartPoint)
                {
                    _startPoint = spawnPoint;
                    ground = _startBlockPrefab;

                    Map.Player = SpawnPlayer(spawnPoint);
                }
                else if (isFinishPoint)
                {
                    _finishPoint = spawnPoint;
                    ground = _finishBlockPrefab;
                }

                Map.GroundBlocks[x, y] = SpawnBlock(ground, spawnPoint);

                if (isFinishPoint == false && isStartPoint == false)
                {
                    if (x == 0 || x == borderedSize.x - 1 || y == 0 || y == borderedSize.y -1)
                    {
                        SpawnBlock(_wallBlockPrefab, spawnPoint);
                    }
                    else if (SampleNoise(x, y) >= _wallSpawnNoiseValue)
                    {
                        SpawnBlock(_wallBlockPrefab, spawnPoint);
                    }
                }
            }
        }
    }

    private void BakeNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }

    private bool CheckPathAvailability(Vector3 sourcePoint, Vector3 targetPoint)
    {
        NavMeshPath path = new NavMeshPath();

        return NavMesh.CalculatePath(sourcePoint, targetPoint, NavMesh.AllAreas, path);
    }

    private float SampleNoise(int x, int y)
    {
        float xPoint = (float)x / _size.x * (_size.x * _noiseScalePercent) + _noiseOffset.x;
        float yPoint = (float)y / _size.y * (_size.y * _noiseScalePercent) + _noiseOffset.y;

        return Mathf.PerlinNoise(xPoint, yPoint);
    }

    private Color SampleNoiseColor(int x, int y)
    {
        float sample = SampleNoise(x, y);

        return new Color(sample, sample, sample);
    }

    private GameObject SpawnBlock(GameObject prefab, Vector3 spawnPoint)
    {
        return _diContainer.InstantiatePrefab(prefab, spawnPoint, Quaternion.identity, Map.Object.transform);
    }

    private Player SpawnPlayer(Vector3 spawnPoint)
    {
        return _diContainer.InstantiatePrefab(_playerPrefab, spawnPoint, Quaternion.identity, Map.Object.transform)
            .GetComponent<Player>();
    }
}
