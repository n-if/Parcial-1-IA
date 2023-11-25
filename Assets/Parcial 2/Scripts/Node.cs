using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Node : MonoBehaviour
{
    public List<Node> _neighbors = new List<Node>();
    public bool Blocked { get { return _blocked; } }
    public int cost;
    bool _blocked;
    Grid _grid;
    int _x, _y;

    public void Initialize(Grid grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;

        SetCost(Random.Range(1, 51));
    }

    public List<Node> GetNeighbors()
    {
        if(_neighbors.Count > 0)
            return _neighbors;

        Node current = _grid.GetNode(_x - 1, _y); //Left
        if (current != null)
            _neighbors.Add(current);

        current = _grid.GetNode(_x + 1, _y); //Right
        if (current != null)
            _neighbors.Add(current);

        current = _grid.GetNode(_x, _y + 1); //Up
        if (current != null)
            _neighbors.Add(current);

        current = _grid.GetNode(_x, _y - 1); //Down
        if (current != null)
            _neighbors.Add(current);

        return _neighbors;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SetStartingNode(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.SetGoalNode(this);
        }
        if (Input.GetMouseButtonDown(2))
        {
            _blocked = !_blocked;

            if (_blocked)
                gameObject.layer = 6;
            else
                gameObject.layer = 0;

            GetComponent<Renderer>().material.color = _blocked ? Color.gray : Color.white;
        }

        if (Input.GetKey(KeyCode.UpArrow))
            SetCost(cost + 1);

        if (Input.GetKey(KeyCode.DownArrow))
            SetCost(cost - 1);

        if (Input.GetKeyDown(KeyCode.R))
            SetCost(1);
    }

    public void SetCost(int newCost)
    {
        cost = Mathf.Clamp(newCost, 1, 99);
        GetComponentInChildren<TextMeshProUGUI>().text = cost + "";
    }
}
