using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Vector3 _velocity;
    public float maxVelocity;
    public float maxForce, flockingMaxForce, arriveMaxForce, evadeMaxForce;
    public float separationRadius, viewRadius, arriveRadius, lookingRadius;

    private Transform _closestPosition;

    public Hunter hunter;

    public Vector3 Velocity { get { return _velocity; } }

    private void Start()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        AddForce(new Vector3(randomX, 0, randomZ) * maxVelocity);


        GameManager.Instance.boids.Add(this);
    }

    int closest;
    private void Update()
    {
        //AddForce(Arrive(target.position));
        //
        //transform.position = GameManager.Instance.ApplyBounds(transform.position + _velocity * Time.deltaTime);
        //transform.forward = _velocity;



        closest = ClosestEntity();

        Debug.Log(closest);

        switch (closest)
        {
            case 0:
                //evade

                Debug.Log("Evadae");
                

                if (maxForce != evadeMaxForce)
                    maxForce = evadeMaxForce;

                AddForce(Evade(hunter.transform.position + hunter.Velocity));

                transform.position = GameManager.Instance.ApplyBounds(transform.position + _velocity * Time.deltaTime);
                transform.forward = _velocity;

                break;

            case 1:
                if (maxForce != arriveMaxForce)
                    maxForce = arriveMaxForce;

                AddForce(Arrive(_closestPosition.position));

                transform.position = GameManager.Instance.ApplyBounds(transform.position + _velocity * Time.deltaTime);
                transform.forward = _velocity;
                break;

            case 2:

                if (maxForce != flockingMaxForce)
                    maxForce = flockingMaxForce;

                Flocking();
                transform.position = GameManager.Instance.ApplyBounds(transform.position + _velocity * Time.deltaTime);

                transform.forward = _velocity;
                break;

            default:
                Debug.LogError("Closest out of index.");
                break;
        }

    }

    #region Flocking
    private void Flocking()
    {
        AddForce(Separation(GameManager.Instance.boids, separationRadius) * GameManager.Instance.separationWeight);
        AddForce(Alignment(GameManager.Instance.boids, viewRadius) * GameManager.Instance.alignmentWeight);
        AddForce(Cohesion(GameManager.Instance.boids, viewRadius) * GameManager.Instance.cohesionWeight);
    }

    private Vector3 Separation(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var boid in boids)
        {
            var dir = boid.transform.position - transform.position;

            if (dir.magnitude > radius || boid == this)
                continue;

            desired -= dir;
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= maxVelocity;


        return CalculateSteering(desired);

    }

    private Vector3 Alignment(List<Boid> boids, float radius)
    {
        var desired = Vector3.zero;
        int count = 0;

        foreach (var boid in boids)
        {
            if (boid == this)
                continue;

            if (Vector3.Distance(transform.position, boid.transform.position) <= radius)
            {
                desired += boid._velocity;
                count++;
            }
        }

        if (count <= 0)
            return Vector3.zero;

        desired /= count;
        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }

    private Vector3 Cohesion(List<Boid> boids, float radius)
    {
        Vector3 desired = transform.position;
        int count = 0;

        foreach (var item in boids)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);

            if (dist > radius || item == this)
                continue;

            desired += item.transform.position;
            count++;
        }

        if (count <= 0)
            return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }

    #endregion

    #region Movement


    public void AddForce(Vector3 dir)
    {
        _velocity += dir;
        _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering;
    }

    Vector3 Arrive(Vector3 target)
    {
        var dist = Vector3.Distance(transform.position, target);

        if (dist > arriveRadius)
            return Seek(target);

        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxVelocity * (dist / arriveRadius);


        return CalculateSteering(desired);
    }

    Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }


    Vector3 Evade(Vector3 target)
    {
        var desired = transform.position - target;
        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }
    #endregion

    #region Closest
    private float _closestDistance;
    private GameObject _closestEntity;
    private int ClosestEntity()
    {
        Collider[] collidersEnRango = Physics.OverlapSphere(transform.position, lookingRadius);

        _closestDistance = Mathf.Infinity;
        _closestEntity = null;
        foreach (var col in collidersEnRango)
        {
            GameObject obj = col.gameObject;

            if (obj.GetComponent<Boid>())
                continue;

            if (obj.GetComponent<Hunter>())
            {
                _closestPosition = obj.transform;
                return 0;
            }

            float distance = (obj.transform.position - gameObject.transform.position).magnitude;
            if (distance < _closestDistance)
            {
                _closestDistance = distance;
                _closestEntity = obj;
            }
        }
        if (!_closestEntity)
            return 2;
        else if (_closestEntity.GetComponent<Food>())
        {
            _closestPosition = _closestEntity.transform;
            return 1;
        }
        else
            return 2;

    }

    #endregion

    public void Die()
    {
        Debug.Log("me mueroo");
        GameManager.Instance.boids.Remove(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, separationRadius);
        //
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, lookingRadius);

    }
}
