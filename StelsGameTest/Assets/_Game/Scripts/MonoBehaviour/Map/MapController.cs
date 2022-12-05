using UnityEngine;
using UnityEngine.AI;
using Zenject;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public event Action OnGeneratedEvent;

    [Header("Prefabs")]
    [SerializeField] private GroundBlock _groundBlockPrefab;
    [SerializeField] private GroundBlock _startBlockPrefab;
    [SerializeField] private GroundBlock _finishBlockPrefab;
    [SerializeField] private GameObject _wallBlockPrefab;
    [Space]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [Header("Generation")]
    [SerializeField] private Vector2Int _size;
    [SerializeField, Range(0, 1)] private float _wallSpawnNoiseValue;
    [SerializeField] private bool _generateOnStart;
    [Header("Eneimies")]
    [SerializeField] private int _enemiesCount;
    [SerializeField] private float _maxEnemyPlayerRange = 5;
    [Header("Noise")]
    [SerializeField] private bool _displayNoise;
    [SerializeField] private Vector2Int _noiseSize;
    [SerializeField, Range(0, 1)] private float _noiseScalePercent;
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

        GenerateEnemies();

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

        if (_displayNoise == false)
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
        bool isStartPoint;
        bool isFinishPoint;
        GroundBlock ground;
        Vector2Int borderedSize = new Vector2Int(_size.x + 2, _size.y + 2);
        Vector3 offset = new Vector2(-(float)borderedSize.x / 2 + BLOCK_RADIUS, -(float)borderedSize.y / 2 + BLOCK_RADIUS);

        Map = new Map(borderedSize, transform);

        for (int x = 0; x < borderedSize.x; x++)
        {
            for (int y = 0; y < borderedSize.y; y++)
            {
                spawnPoint = new Vector3(x + offset.x, 0, y + offset.y);

                ground = _groundBlockPrefab;

                isStartPoint = x == 1 && y == 1;
                isFinishPoint = x == borderedSize.x - 1 && y == borderedSize.y - 2;

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

                Map.GroundBlocks[x, y] = SpawnBlock(ground.gameObject, spawnPoint).GetComponent<GroundBlock>();

                if (isFinishPoint == false && isStartPoint == false)
                {
                    if (x == 0 || x == borderedSize.x - 1 || y == 0 || y == borderedSize.y - 1)
                    {
                        SpawnBlock(_wallBlockPrefab, spawnPoint);
                    }
                    else if (SampleNoise(x, y) >= _wallSpawnNoiseValue)
                    {
                        SpawnBlock(_wallBlockPrefab, spawnPoint);
                    }
                    else Map.GroundBlocks[x, y].IsFree = true;
                }
            }
        }
    }

    private void GenerateEnemies()
    {
        List<GroundBlock> freeBlocks = Map.GroundBlocks.Cast<GroundBlock>().ToList()
            .FindAll(block => block.IsFree);

        List<GroundBlock> availableBlocks = freeBlocks.FindAll(block =>
            Vector3.Distance(block.transform.position, Map.Player.transform.position) > _maxEnemyPlayerRange);

        availableBlocks = availableBlocks.FindAll(block => CheckPathAvailability(block.transform.position, Map.Player.transform.position));

        for (int i = 0; i < _enemiesCount; i++)
        {
            //bool isBlockFound = false;

            if(availableBlocks.Count == 0)
            {
                Debug.LogWarning("[Map Controller] Enemies can`t be generated. There is no Avilable blocks");
                return;
            }

            GroundBlock block = availableBlocks.Random();

            /*while (isBlockFound == false)
            {
                block = availableBlocks.Random();

                isBlockFound = CheckPathAvailability(block.transform.position, Map.Player.transform.position);
            }*/

            Map.Enemies.Add(SpawnEnemy(block.transform.position));
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

    private Enemy SpawnEnemy(Vector3 spawnPoint)
    {
        return _diContainer.InstantiatePrefab(_enemyPrefab, spawnPoint, Quaternion.identity, Map.Object.transform)
            .GetComponent<Enemy>();
    }
}
