using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node prefab;
    public int width, height; 
    public float offset;
    Node[,] _grid;

    void Start()
    {
        _grid = new Node[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var go = Instantiate(prefab);
                go.Initialize(this, x, y);
                go.transform.position = new Vector3(x * go.transform.localScale.x, y * go.transform.localScale.y, 0) * offset;
                _grid[x,y] = go;
            }
        }     
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return null;

        return _grid[x, y];
    }
}
