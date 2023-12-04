using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    public List<Boid> boids = new List<Boid>();
    public int width, height;

    [Range(0, 4f)]
    public float separationWeight, cohesionWeight, alignmentWeight = 1;

    public GameObject foodPrefab;
    public float foodSpawnRate;
    private float _timer;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > foodSpawnRate)
        {
            SpawnFood();
            _timer = 0;
        }
    }

    public Vector3 ApplyBounds(Vector3 pos)
    {
        if (pos.x > width)
            pos.x = -width;
        else if (pos.x < -width)
            pos.x = width;

        if (pos.z > height)
            pos.z = -height;
        else if (pos.z < -height)
            pos.z = height;

        return pos;
    }

    private float randomPosX, randomPosZ;
    private Vector3 randomPos;
    public void SpawnFood()
    {
        randomPosX = Random.Range(-width, width);
        randomPosZ = Random.Range(-height, height);

        randomPos = new Vector3 (randomPosX, 0, randomPosZ);

        Instantiate(foodPrefab, randomPos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(75, 0, 130);

        Vector3 topLeft = new Vector3(-width, 0, height);
        Vector3 topRight = new Vector3(width, 0, height);
        Vector3 bottomLeft = new Vector3(-width, 0, -height);
        Vector3 bottomRight = new Vector3(width, 0, -height);

        Gizmos.DrawWireCube(topLeft, Vector3.one);
        Gizmos.DrawWireCube(topRight, Vector3.one);
        Gizmos.DrawWireCube(bottomLeft, Vector3.one);
        Gizmos.DrawWireCube(bottomRight, Vector3.one);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
    }
}
