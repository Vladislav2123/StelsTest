using UnityEngine;
using System.Collections.Generic;

public class Map
{
    public Vector2Int Size { get; private set; }
    public GameObject Object { get; private set; }
    public GroundBlock[,] GroundBlocks { get; }

    public Player Player { get; set; }
    public List<Enemy> Enemies { get; private set; }

    private Bounds _bounds;
    public Bounds Bounds
    {
        get
        {
            _bounds = new Bounds();
            _bounds.Encapsulate(GroundBlocks[0, 0].transform.position);
            _bounds.Encapsulate(GroundBlocks[Size.x - 1, Size.y - 1].transform.position);

            return _bounds;
        }
    }

    public Map(Vector2Int size, Transform parent)
    {
        Size = size;

        Object = new GameObject("Map");
        Object.transform.SetParent(parent);

        GroundBlocks = new GroundBlock[Size.x, Size.y];
        Enemies = new List<Enemy>();
    }
}
