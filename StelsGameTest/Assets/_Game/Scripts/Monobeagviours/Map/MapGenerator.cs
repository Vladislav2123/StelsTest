using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _startPrefab;
    [SerializeField] private GameObject _finishPrefab;
    [Header("Generation")]
    [SerializeField] private Vector2Int _size;
    [SerializeField, Range(0, 1)] private float _wallSpawnNoiseValue;
    [Header("Noise")]
    [SerializeField] private Vector2Int _noiseSize;
    [SerializeField, Range(0,1)] private float _noiseScalePercent;
    [SerializeField] private Vector2 _maxNoiseOffset = new Vector2(99999, 99999);
    [SerializeField] private RawImage _noiseDisplay;
    [SerializeField] private Button _generateButton;

    private Vector2 _noiseOffset;
    private GameObject _map;

    private void Start()
    {
        GenerateMap();

        _generateButton.onClick.AddListener(GenerateMap);
    }

    private void GenerateMap()
    {
        ResetMap();
        GenerateNoise();
        GenerateModel();
    }

    private void GenerateNoise()
    {
        _noiseOffset = new Vector2(Random.Range(0, _maxNoiseOffset.x), Random.Range(0, _maxNoiseOffset.y));

        Texture2D noiseTexture = new Texture2D(_noiseSize.x, _noiseSize.y);

        for (int x = 0; x < _noiseSize.x; x++)
        {
            for (int y = 0; y < _noiseSize.y; y++)
            {
                noiseTexture.SetPixel(x, y, SampleNoiseColor(x, y));
            }
        }

        noiseTexture.Apply();
        _noiseDisplay.texture = noiseTexture;
    }

    private void GenerateModel()
    {
        Vector3 spawnPoint;
        Vector3 offset = new Vector2(-(float)_size.x / 2, -(float)_size.y / 2);

        _map = new GameObject("Map");
        _map.transform.SetParent(transform);

        for (int x = -1; x < _size.x + 1; x++)
        {
            for (int y = -1; y < _size.y + 1; y++)
            {
                spawnPoint = new Vector3(x + offset.x, 0, y + offset.y);

                GameObject ground = _groundPrefab;

                bool isStartPoint = x == 0 && y == 0;
                bool isFinishPoint = x == _size.x && y == _size.y - 1;

                if (isStartPoint) ground = _startPrefab;
                else if (isFinishPoint) ground = _finishPrefab;

                SpawnCell(ground, spawnPoint);

                if (isFinishPoint == false && isStartPoint == false)
                {
                    if ((isFinishPoint == false) && (x == -1 || x == _size.x || y == -1 || y == _size.y))
                    {
                        SpawnCell(_wallPrefab, spawnPoint);
                    }
                    else if (SampleNoise(x, y) >= _wallSpawnNoiseValue)
                    {
                        SpawnCell(_wallPrefab, spawnPoint);
                    }
                }
            }
        }
    }

    private void ResetMap()
    {
        if (_map != null) Destroy(_map);
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

    private void SpawnCell(GameObject prefab, Vector3 spawnPoint)
    {
        Instantiate(prefab, spawnPoint, Quaternion.identity, _map.transform);
    }
}
