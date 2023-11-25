using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LayerMask maskWall;
    public Player player;
    Node _startingNode;
    Node _goalNode;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _startingNode != null && _goalNode != null)
        {
            player.SetPath(Pathfinding.instance.CalculateThetaStar(_startingNode, _goalNode));
            //StartCoroutine(Pathfinding.instance.CoroutineCalculateThetaStar(_startingNode, _goalNode));
        }
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, maskWall);
    }

    public void SetStartingNode(Node node)
    {
        if (_startingNode != null)
            ChangeColor(_startingNode, Color.white);

        _startingNode = node;
        ChangeColor(_startingNode, Color.red);
        player.transform.position = _startingNode.transform.position;
    }

    public void SetGoalNode(Node node)
    {
        if (_goalNode != null)
            ChangeColor(_goalNode, Color.white);

        _goalNode = node;
        ChangeColor(_goalNode, Color.green);
    }

    public void ChangeColor(Node node, Color color)
    {
        node.GetComponent<Renderer>().material.color = color;
    }
}
