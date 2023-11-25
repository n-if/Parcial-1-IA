using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> CalculateDijkstra(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while(frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if(current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (item.Blocked)
                    continue;

                int newCost = costSoFar[current] + item.cost;

                if (!costSoFar.ContainsKey(item))
                {
                    frontier.Enqueue(item, newCost);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if(costSoFar[item] > newCost)
                {
                    frontier.Enqueue(item, newCost);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
        }
        return new List<Node>();
    }

    public List<Node> CalculateBFS(Node startingNode, Node goalNode)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if(current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach(var item in current.GetNeighbors())
            {
                if(!item.Blocked && !cameFrom.ContainsKey(item))
                {
                    frontier.Enqueue(item);
                    cameFrom.Add(item, current);
                }
            }
        }

        return new List<Node>();
    }

    public List<Node> CalculateAStar(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (item.Blocked)
                    continue;

                int newCost = costSoFar[current] + item.cost;
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(item))
                {
                    if(!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost )
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
        }
        return new List<Node>();
    }

    public List<Node> CalculateThetaStar(Node startingNode, Node goalNode)
    {
        var listNode = CalculateAStar(startingNode, goalNode);

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            if (GameManager.instance.InLineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position))
            {
                listNode.RemoveAt(current + 1);
            }
            else
                current++;
        }

        return listNode;
    }

    public IEnumerator CoroutineCalculateDijkstra(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            yield return new WaitForSeconds(0.1f);
            GameManager.instance.ChangeColor(current, Color.blue);

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];

                    yield return new WaitForSeconds(0.1f);
                    GameManager.instance.ChangeColor(current, Color.yellow);
                }

            }

            foreach (var item in current.GetNeighbors())
            {
                if (item.Blocked)
                    continue;

                int newCost = costSoFar[current] + item.cost;

                if (!costSoFar.ContainsKey(item))
                {
                    frontier.Enqueue(item, newCost);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost)
                {
                    frontier.Enqueue(item, newCost);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }

                yield return new WaitForSeconds(0.02f);
                GameManager.instance.ChangeColor(current, Color.cyan);
            }
        }
        
    }

    public IEnumerator CoroutineCalculateBFS(Node startingNode, Node goalNode)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            GameManager.instance.ChangeColor(current, Color.blue);
            yield return new WaitForSeconds(0.1f);

            if (current == goalNode)
            {
                while (current != startingNode)
                {
                    GameManager.instance.ChangeColor(current, Color.yellow);
                    yield return new WaitForSeconds(0.1f);
                    current = cameFrom[current];
                }

                break;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (!item.Blocked && !cameFrom.ContainsKey(item))
                {
                    frontier.Enqueue(item);
                    cameFrom.Add(item, current);

                    GameManager.instance.ChangeColor(current, Color.cyan);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    public IEnumerator CoroutineCalculateGreedyBFS(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            GameManager.instance.ChangeColor(current, Color.blue);
            yield return new WaitForSeconds(0.1f);

            if (current == goalNode)
            {
                while (current != startingNode)
                {
                    GameManager.instance.ChangeColor(current, Color.yellow);
                    yield return new WaitForSeconds(0.1f);
                    current = cameFrom[current];
                }

                break;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (!item.Blocked && !cameFrom.ContainsKey(item))
                {
                    float priority = Vector3.Distance(item.transform.position, goalNode.transform.position);
                    frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);

                    GameManager.instance.ChangeColor(current, Color.cyan);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    public IEnumerator CoroutineCalculateAStar(Node startingNode, Node goalNode)
    {

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];

                    yield return new WaitForSeconds(0.1f);
                    GameManager.instance.ChangeColor(current, Color.yellow);
                }

                break;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (item.Blocked)
                    continue;

                int newCost = costSoFar[current] + item.cost;
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(item) )
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost )
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }

                yield return new WaitForSeconds(0.02f);
                GameManager.instance.ChangeColor(current, Color.cyan);
            }
        }  
    }

    public IEnumerator CoroutineCalculateThetaStar(Node startingNode, Node goalNode)
    {
        var listNode = CalculateAStar(startingNode, goalNode);

        foreach (var item in listNode)
        {
            GameManager.instance.ChangeColor(item, Color.blue);
        }

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            GameManager.instance.ChangeColor(listNode[current], Color.red);
            GameManager.instance.ChangeColor(listNode[current + 2], Color.green);

            yield return new WaitForSeconds(0.7f);

            if (GameManager.instance.InLineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position))
            {
                GameManager.instance.ChangeColor(listNode[current + 1], Color.white);
                listNode.RemoveAt(current + 1);
            }
            else
                current++;
        }

        
    }

}
